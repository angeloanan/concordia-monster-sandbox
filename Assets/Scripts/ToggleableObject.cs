using UnityEngine;

public class ToggleableObject : MonoBehaviour {
  [SerializeField] private GameObject off;
  [SerializeField] private GameObject on;

  private bool isOn = false;
  
  void Start() {
    off.SetActive(true);
    on.SetActive(false);
  }

  void Update() {
    
  }

  public void Toggle() {
    isOn = !isOn;
    off.SetActive(!isOn);
    on.SetActive(isOn);
  }
  public void OnMouseDown() {
    Toggle();
  }
}