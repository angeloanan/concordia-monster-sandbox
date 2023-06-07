using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameWorldBehavior : MonoBehaviour {
  [SerializeField] public bool canMoveCamera = true;
  [SerializeField] private float cameraSensitivity = 0.01f;
  [SerializeField] private int cameraClampDepth = 10;
  [SerializeField] private int cameraClampWidth = 10;

  private Vector2 _pointerDelta;
  private Vector2 _pointerInitialPosition;
  private bool _isClicking;
  
  private GameObject _movingObject;
  private Vector3 _initialObjectScreenPosition;


  public void Awake() {
    var monster = MonsterDataManager.Instance.activeMonsterPrefab;
    
    monster.transform.position = new Vector3(0, 1, 5);
    monster.transform.localScale = new Vector3(1, 1, 1);
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
      var position = Vector3.right * (_pointerDelta.x * -cameraSensitivity);
      position += Vector3.forward * (_pointerDelta.y * -cameraSensitivity);

      // transform.position += position * Time.deltaTime;
      // ^ Goes crazy on lag
      Camera.main.transform.position += position;

      // Clamp camera position
      var clampedPosition = Camera.main.transform.position;
      clampedPosition.x = Mathf.Clamp(clampedPosition.x, -cameraClampWidth, cameraClampWidth);
      clampedPosition.z = Mathf.Clamp(clampedPosition.z, -cameraClampDepth, cameraClampDepth);
      Camera.main.transform.position = clampedPosition;
    }
  }
}