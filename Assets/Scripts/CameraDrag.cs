using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Deprecated - Use <see cref="CursorDragSystem"/> instead.
/// </summary>
public class CameraDrag : MonoBehaviour {
  [SerializeField] private float sensitivity = 2f;

  private Vector2 _delta;
  private bool _isMoving;

  public void onLook(InputAction.CallbackContext ctx) {
    _delta = ctx.ReadValue<Vector2>();
  }

  public void onMove(InputAction.CallbackContext ctx) {
    _isMoving = (ctx.started || ctx.performed) && !ctx.canceled;
  }
  
  private void LateUpdate() {
    if (!_isMoving) return;
    
    var position = transform.right * (_delta.x * -sensitivity);
    position += transform.forward * (_delta.y * -sensitivity);
    
    // transform.position += position * Time.deltaTime;
    // ^ Goes crazy on lag
    transform.position += position;
  }
}