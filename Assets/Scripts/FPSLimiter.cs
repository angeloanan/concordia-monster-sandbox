using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour {
  void Awake() {
    Application.targetFrameRate = 30;
  }
}