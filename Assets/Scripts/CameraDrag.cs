using UnityEngine;

public class CameraDrag : MonoBehaviour {
  [SerializeField] private float sensitivity = 2f;

  private float sensitivityConstant = 1f;

  private Vector3 dragOrigin;

  private void OnMouseDown() {
    dragOrigin = Input.mousePosition;
  }

  private void OnMouseDrag() {
    Vector3 mouseDelta = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
    Vector3 movement = new Vector3(mouseDelta.x * sensitivity, 0, mouseDelta.y * sensitivity);

    transform.Translate(movement, Space.World);
  }
}