using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Debug = System.Diagnostics.Debug;

public class CursorDragSystem : MonoBehaviour {
  [SerializeField] public bool canMoveCamera = true;
  [SerializeField] private float cameraSensitivity = 0.01f;
  
  private Vector2 _delta;
  private bool _isClicking;
  private GameObject _movingObject;
  private Vector3 _initialWorldMousePosition;
  
  // OnMouseDrag
  public void OnLook(InputAction.CallbackContext ctx) {
    _delta = ctx.ReadValue<Vector2>();
  }

  public void OnStopLooking() {
    // Cleanup
    _isClicking = false;
    _movingObject = null;
    _initialWorldMousePosition = Vector3.zero;
  }

  // OnClick
  public void OnMove(InputAction.CallbackContext ctx) {
    Debug.Assert(Camera.main != null, "Camera.main != null");

    if (ctx.performed) return;
    if (ctx.canceled) {
      OnStopLooking();
      return;
    }
    _isClicking = true;
    Debug.Print("I'm here!");
    
    // Raytrace cursor position to world and check if it hits anything
    // If it hits something, then set the object to be dragged
    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    Physics.Raycast(ray, out var hit, 100);
    if (hit.collider != null && hit.transform.CompareTag("Draggable")) {
      // Hits a draggable object
       _initialWorldMousePosition =  Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
      _movingObject = hit.transform.gameObject;
    } else {
      // Handle camera move
      _movingObject = null; // Sanity check, shouldn't need this but whatever
    }
  }

  private void LateUpdate() {
    Debug.Assert(Camera.main != null, "Camera.main != null");
    if (!_isClicking) return;
    
    if (_movingObject != null) { // Moving object
      _movingObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - _initialWorldMousePosition);
    } else { // Moving camera
      var position = transform.right * (_delta.x * -cameraSensitivity);
      position += transform.forward * (_delta.y * -cameraSensitivity);

      // transform.position += position * Time.deltaTime;
      // ^ Goes crazy on lag
      transform.position += position;
    }
  }
}