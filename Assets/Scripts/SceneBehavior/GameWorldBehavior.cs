using System;
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
  
  private GameObject _movingObject;
  private Vector3 _initialObjectScreenPosition;

  public void Awake() {
    var monster = MonsterDataManager.Instance.activeMonsterPrefab;
    // If null then LateUpdate doesn't trigger :german:
    if (monster != null) {
      monster.transform.position = new Vector3(0, 1, 5);
      monster.transform.localScale = new Vector3(1, 1, 1);
    }
  }

  public void OnStopLooking() {
    // Cleanup
    _isClicking = false;
    _movingObject = null;
  }

  // OnMouseDrag
  public void OnPointerDelta(InputAction.CallbackContext ctx) {
    _pointerDelta = ctx.ReadValue<Vector2>();
    Debug.Log("Pointer delta: " + _pointerDelta);
  }
  
  public void OnPointerInitialPosition(InputAction.CallbackContext ctx) {
    _pointerInitialPosition = ctx.ReadValue<Vector2>();
    Debug.Log("Pointer initial position: " + _pointerInitialPosition);
  }

  // OnClick
  public void OnPointerClick(InputAction.CallbackContext ctx) {
    Debug.Assert(Camera.main != null, "Camera.main != null");
    Debug.Log("Pointer click: " + ctx.ReadValueAsObject());

    if (ctx.performed) return;
    if (ctx.canceled) {
      Debug.Log("Canceled click");
      OnStopLooking();
      return;
    }
    
    _isClicking = true;

    // Raytrace cursor position to world and check if it hits anything
    // If it hits something, then set the object to be dragged
    var ray = Camera.main.ScreenPointToRay(_pointerInitialPosition);
    Physics.Raycast(ray, out var hit, 100);
    Debug.DrawRay(_pointerInitialPosition, ray.direction * 100, Color.yellow, 30);
    Debug.Log("Hit: " + hit.transform?.name ?? "null");
    if (hit.collider != null && hit.transform.CompareTag("Draggable")) {
      // Hits a draggable object
      _movingObject = hit.transform.gameObject;

      _initialObjectScreenPosition = Camera.main.WorldToScreenPoint(_movingObject.transform.position);
    }
    else {
      // Handle camera move
      _movingObject = null; // Sanity check, shouldn't need this but whatever
    }
  }

  private void LateUpdate() {
    Debug.Assert(Camera.main != null, "Camera.main != null");
    // Only handle drag stuff if we're clicking
    if (!_isClicking) return;

    if (_movingObject != null) { // Moving object
      Vector3 mouseDelta = _pointerDelta;
      Vector3 targetObjectScreenPosition =
        _initialObjectScreenPosition + new Vector3(mouseDelta.x, mouseDelta.y, mouseDelta.z);
      // _movingObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - _initialWorldMousePosition);
    }
    else { // Moving camera
      float rotationAroundXAxis = _pointerDelta.y * cameraSensitivity;
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