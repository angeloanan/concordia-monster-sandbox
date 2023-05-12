using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleableObject : MonoBehaviour {
  [SerializeField] private GameObject off;
  [SerializeField] private GameObject on;

  private bool isOn = false;
  
  void Start() {
    off.SetActive(true);
    on.SetActive(true);
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