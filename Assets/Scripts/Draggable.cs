using UnityEngine;

public class Draggable : MonoBehaviour {
    private Vector3 _mousePosition;

    private Vector3 GetMousePos() {
        return Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown() {
        _mousePosition = Input.mousePosition - GetMousePos();
    }

    private void OnMouseDrag() {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - _mousePosition);
    }
}
