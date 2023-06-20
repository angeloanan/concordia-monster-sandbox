using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameWorldBehavior : MonoBehaviour {
  [SerializeField] private UIDocument uiDocument;
  [SerializeField] public bool canMoveCamera = true;
  [SerializeField] private float cameraSensitivity = 0.1f;
  [SerializeField] private Vector3 cameraCenter = new Vector3(0, 0, 0);
  [SerializeField] private float cameraRadiusFromCenter = 20.0f;
  [SerializeField] private float cameraClampRotationDeg = 45.0f;

  private Vector2 _pointerDelta;
  private Vector2 _pointerInitialPosition;

  private bool _isClicking;

  /// <summary>
  /// Tracking whether camera did move within a life-cycle of a thing
  /// </summary>
  private bool _cameraDidMove;

  private GameObject _activeObject;
  private Vector3 _initialObjectScreenPosition;

  public void Awake() {
    var monster = MonsterDataManager.Instance.activeMonsterPrefab;

    // Place monster on default position
    // If null then LateUpdate doesn't trigger :german:
    if (monster != null) {
      monster.transform.position = new Vector3(0, 1, 5);
      monster.transform.localScale = new Vector3(1, 1, 1);
      monster.transform.LookAt(Camera.main.transform.position);
    }
  }

  /// <summary>
  /// Utility function to clean-up camera's internal state.
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
    }

    _cameraDidMove = false;
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
      var deleteButton = root.Q<VisualElement>("DeleteObjectButton");
      deleteButton.RegisterCallback<ClickEvent>(_ => {
        Destroy(newActiveObject);
        ChangeActiveObject(null);
      });
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

    _isClicking = true;
  }

  // Ran on the end of every frame
  private void LateUpdate() {
    Debug.Assert(Camera.main != null, "Camera.main != null");

    UpdateCameraPosition();
  }

  private void UpdateCameraPosition() {
    // Only handle drag stuff if we're clicking
    if (!_isClicking) return;

    if (!_cameraDidMove && _pointerDelta.magnitude > 1f) _cameraDidMove = true;
    var rotationAroundXAxis = -_pointerDelta.y * cameraSensitivity;
    var rotationAroundYAxis = _pointerDelta.x * cameraSensitivity;

    // TP Camera to center position
    var cam = Camera.main;
    cam.transform.position = cameraCenter;
    cam.transform.Rotate(Vector3.right, rotationAroundXAxis);
    cam.transform.Rotate(Vector3.up, rotationAroundYAxis, Space.World);

    // Clamp rotation
    var currentRotation = cam.transform.rotation.eulerAngles;
    currentRotation.x = Mathf.Clamp(currentRotation.x, -cameraClampRotationDeg, cameraClampRotationDeg);
    cam.transform.rotation = Quaternion.Euler(currentRotation);

    cam.transform.Translate(new Vector3(0, 0, -cameraRadiusFromCenter));
  }

  private void updateActiveObjectUIElement() {
    if (_activeObject == null) return;
    var root = uiDocument.rootVisualElement;
    var button = root.Q<VisualElement>("DeleteObjectButton");

    if (button == null) {
      Debug.LogWarning("DeleteObjectButton not found in UI but active object is not null.");
      return;
    }

    var activeObjectScreenPos = Camera.main.WorldToScreenPoint(_activeObject.transform.position);
    // Debug.Log($"Active object screen pos: {activeObjectScreenPos}");
    button.style.left = activeObjectScreenPos.x / 2;
    // TODO: Fix this
    // button.style.top = activeObjectScreenPos.y;
  }

  private void Update() {
    updateActiveObjectUIElement();
  }
}