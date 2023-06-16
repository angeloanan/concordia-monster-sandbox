using UnityEngine;
using UnityEngine.InputSystem;

public class GameWorldBehavior : MonoBehaviour {
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
    }
  }

  /// <summary>
  /// Utility function to clean-up camera's internal state.
  ///
  /// Could be better represented via a State-Machine of some sort, but idk how to do that and cba to learn.
  /// </summary>
  private void OnStopLooking() {
    Debug.Log($"Camera did move: {_cameraDidMove}");
    _isClicking = false;
    
    // If an object is selected WHILE camera is being moved, then don't deselect the camera
    if (!_cameraDidMove) {
      _activeObject = null;
      _cameraDidMove = false;
    }
  }

  private void ChangeActiveObject(GameObject newActiveObject) {
    if (_activeObject != null) {
      // Cleanup old object
      // 1. Remove outline shader
      // 2. Remove mounted UI element

      _activeObject = null;
    }

    if (newActiveObject != null) {
      // Assign new object to stuff
      // 1. Add outline shader
      // 2. Mount UI element

      _activeObject = newActiveObject;
    }
  }

  // OnMouseDrag
  public void OnPointerDelta(InputAction.CallbackContext ctx) {
    _pointerDelta = ctx.ReadValue<Vector2>();
  }
  public void OnPointerInitialPosition(InputAction.CallbackContext ctx) {
    _pointerInitialPosition = ctx.ReadValue<Vector2>();
  }

  // OnClick
  public void OnPointerClick(InputAction.CallbackContext ctx) {
    Debug.Assert(Camera.main != null, "Camera.main != null");

    if (ctx.performed) return;
    if (ctx.canceled) {
      // Stop clicking / holding M1
      OnStopLooking();
      return;
    }

    _isClicking = true;

    // Raytrace cursor position to world and check if it hits anything
    // If it hits something, then set the object to be dragged
    var ray = Camera.main.ScreenPointToRay(_pointerInitialPosition);
    Physics.Raycast(ray, out var hit, 100);
    Debug.DrawRay(_pointerInitialPosition, ray.direction * 100, Color.yellow, 30);
    if (hit.collider != null && hit.transform.CompareTag("Draggable")) {
      ChangeActiveObject(hit.transform.gameObject);
    }
    else {
      // Handle camera move
      _activeObject = null; // Sanity check, shouldn't need this but whatever
    }
  }

  private void LateUpdate() {
    Debug.Assert(Camera.main != null, "Camera.main != null");
    
    UpdateCameraPosition();
  }

  private void UpdateCameraPosition() {
    // Only handle drag stuff if we're clicking
    if (!_isClicking) return;

    if (_activeObject != null) { // Moving object
      // TODO: stuff (See ENG-38)
    }
    else { // Moving camera
      if (!_cameraDidMove && _pointerDelta.magnitude > 1f) _cameraDidMove = true;
      float rotationAroundXAxis = -_pointerDelta.y * cameraSensitivity;
      float rotationAroundYAxis = _pointerDelta.x * cameraSensitivity;

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
  }
}