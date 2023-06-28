using RuntimeHandle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameWorldBehavior : MonoBehaviour {
  [SerializeField] private UIDocument uiDocument;
  [SerializeField] private GameObject transformGizmo;

  [SerializeField] private float cameraSensitivity = 0.1f;
  [SerializeField] private Vector3 cameraCenter = new(0, 0, 0);
  [SerializeField] private float cameraRadiusFromCenter = 15.0f;
  [SerializeField, Range(0, 180)] private float cameraClampRotationDeg = 45.0f;

  private Vector2 _pointerDelta;
  private Vector2 _pointerInitialPosition;

  [SerializeField] private bool canMoveCamera = true;
  private bool _isClicking;

  /// <summary>
  /// Tracking whether camera did move within a life-cycle of a thing
  /// </summary>
  private bool _cameraDidMove;

  private GameObject _activeObject;
  private Vector3 _initialObjectScreenPosition;

  private static void SetLayerAllChildren(Transform root, string layerName) {
    root.gameObject.layer = LayerMask.NameToLayer(layerName);
    foreach (Transform child in root) {
      SetLayerAllChildren(child, layerName);
    }
  }
  
  public void Awake() {
    Debug.Assert(transformGizmo, $"Transform gizmo is not assigned in {name}");
    var monster = MonsterDataManager.Instance.activeMonsterPrefab;

    // Place monster on default position
    // If null then LateUpdate doesn't trigger :german:
    if (monster != null) {
      monster.transform.position = new Vector3(0, 1, 5);
      monster.transform.localScale = new Vector3(1, 1, 1);
      // monster.transform.LookAt(Camera.main.transform.position);
    }

    // Initialize transform gizmo
    var transformHandle = transformGizmo.GetOrAddComponent<RuntimeTransformHandle>();
    transformHandle.type = HandleType.POSITION;
    transformHandle.autoScale = true;
    transformHandle.autoScaleFactor = 1.5F;
    transformHandle.axes = HandleAxes.XYZ;
    transformHandle.target = null;

    SetLayerAllChildren(transformGizmo.transform, "UI");

    transformGizmo.SetActive(false);
  }

  /// <summary>
  /// Utility function to clean-up camera's internal state.
  /// Ran only once AFTER mouse button is UP
  /// Also serves as the "onClick" callback for the camera.
  ///
  /// Could be better represented via a State-Machine of some sort, but idk how to do that and cba to learn.
  /// </summary>
  private void OnStopLooking() {
    _isClicking = false;

    // If an object is selected WHILE camera is being moved, then don't deselect the camera
    // Do whatever onClick callbacks here
    if (!_cameraDidMove) {
      // Raytrace cursor position to world and check if it hits anything
      // If it hits something, then set the object to be dragged
      var ray = Camera.main.ScreenPointToRay(_pointerInitialPosition);
      Physics.Raycast(ray, out var hit, 100);
      Debug.DrawRay(_pointerInitialPosition, ray.direction * 100, Color.yellow, 30);
      if (hit.collider != null && hit.transform.CompareTag("Draggable")) {
        ChangeActiveObject(hit.transform.gameObject);
      }
      else {
        ChangeActiveObject(null);
      }
    }

    _cameraDidMove = false;
    canMoveCamera = true;
  }

  /// <summary>
  /// Utility function to switch the active object to a new one.
  /// </summary>
  /// <param name="newActiveObject">Objects to be newly selected</param>
  private void ChangeActiveObject(GameObject newActiveObject) {
    if (_activeObject == newActiveObject) return;
    var root = uiDocument.rootVisualElement;

    if (_activeObject != null) {
      // Cleanup old object
      // 1. Remove outline shader
      if (_activeObject.TryGetComponent<Outline>(out var outline)) {
        Destroy(outline);
      }

      // 2. Remove mounted UI element
      var deleteButton = root.Q<VisualElement>("DeleteObjectButton");
      root.Remove(deleteButton);

      // 3. Remove Runtime Transform Handles
      transformGizmo.GetComponent<RuntimeTransformHandle>().target = null;
      transformGizmo.SetActive(false);
    }

    if (newActiveObject != null) {
      // Assign new object to stuff
      // 1. Add outline shader
      var outline = newActiveObject.AddComponent<Outline>();

      outline.OutlineMode = Outline.Mode.OutlineAll;
      outline.OutlineColor = Color.yellow;
      outline.OutlineWidth = 5f;

      // 2. Mount UI element
      Resources.Load<VisualTreeAsset>("UI/Components/DeleteObjectButton").CloneTree(root);
      var deleteButton = root.Q<Button>("DeleteObjectButton");
      deleteButton.RegisterCallback<ClickEvent>(_ => { AudioManager.Instance.PlayUiClick(); });
      deleteButton.RegisterCallback<ClickEvent>(_ => {
        Destroy(newActiveObject);
        ChangeActiveObject(null);
      });
      
      var toggleButton = root.Q<Button>("ToggleObjectButton");
      toggleButton.RegisterCallback<ClickEvent>(_ => { AudioManager.Instance.PlayUiClick(); });
      toggleButton.RegisterCallback<ClickEvent>(_ => {
        transformGizmo.GetComponent<RuntimeTransformHandle>().type = HandleType.ROTATION;
      });

      // 3. Add Runtime Transform Handles
      transformGizmo.SetActive(true);
      transformGizmo.GetComponent<RuntimeTransformHandle>().target = newActiveObject.transform;
      
      // 4. Set Runtime Transform Handle layer to UI
      SetLayerAllChildren(transformGizmo.transform, "UI");
    }

    _activeObject = newActiveObject;
  }

  // OnLook - Run every frame
  public void OnPointerDelta(InputAction.CallbackContext ctx) {
    _pointerDelta = ctx.ReadValue<Vector2>();
  }

  // OnMouseMove - Run every frame
  public void OnPointerInitialPosition(InputAction.CallbackContext ctx) {
    _pointerInitialPosition = ctx.ReadValue<Vector2>();
  }

  // OnClick - Run every frame
  public void OnPointerClick(InputAction.CallbackContext ctx) {
    Debug.Assert(Camera.main != null, "Camera.main != null");

    if (ctx.performed) return;
    if (ctx.canceled) {
      // Stop clicking / holding M1
      OnStopLooking();
      return;
    }

    // Check if we're clicking on UI
    var ray = Camera.main.ScreenPointToRay(_pointerInitialPosition);
    if (Physics.Raycast(ray, out var hit, 100F, LayerMask.NameToLayer("UI"))) {
      canMoveCamera = false;
    }

    _isClicking = true;
  }

  // Ran on the end of every frame
  private void LateUpdate() {
    Debug.Assert(Camera.main != null, "Camera.main != null");

    UpdateCameraPosition();
    UpdateActiveObjectUIElement();
  }

  private void UpdateCameraPosition() {
    // Only handle drag stuff if we're clicking
    if (!_isClicking) return;
    if (!canMoveCamera) return;

    if (!_cameraDidMove && _pointerDelta.magnitude > 5f) _cameraDidMove = true;
    // Debug.Log($"Pointer Delta Magnitude: {_pointerDelta.magnitude}");
    var rotationAroundXAxis = -_pointerDelta.y * cameraSensitivity;
    var rotationAroundYAxis = _pointerDelta.x * cameraSensitivity;

    // TP Camera to center position
    var cam = Camera.main;
    cam.transform.position = cameraCenter;
    cam.transform.Rotate(Vector3.right, rotationAroundXAxis);
    cam.transform.Rotate(Vector3.up, rotationAroundYAxis, Space.World);

    // Clamp rotation
    // var currentRotation = cam.transform.rotation.eulerAngles;
    // currentRotation.x = Mathf.Clamp(currentRotation.x, -cameraClampRotationDeg, cameraClampRotationDeg);
    // cam.transform.rotation = Quaternion.Euler(currentRotation);
    
    cam.transform.Translate(new Vector3(0, 0, -cameraRadiusFromCenter));
  }

  private void UpdateActiveObjectUIElement() {
    if (_activeObject == null) return;
    var root = uiDocument.rootVisualElement;
    var button = root.Q<VisualElement>("DeleteObjectButton");

    if (button == null) {
      Debug.LogWarning("DeleteObjectButton not found in UI but active object is not null.");
      return;
    }

    var _activeObjectCollider = _activeObject.GetComponent<Collider>();
    var _activeObjectCenter = _activeObjectCollider.bounds.center;
    var _activeObjectSize = _activeObjectCollider.bounds.size;

    var _activeObjectPostion = (_activeObjectSize / 2) + _activeObjectCenter;

    var activeObjectScreenPos = Camera.main.WorldToScreenPoint(_activeObjectPostion);
    // Debug.Log($"Active object screen pos: {activeObjectScreenPos}");
    button.style.left = activeObjectScreenPos.x;
    // TODO: Fix this
    button.style.bottom = activeObjectScreenPos.y;
  }
}