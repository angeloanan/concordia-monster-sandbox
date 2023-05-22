using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomization : MonoBehaviour {
  public GameObject[] body;
  private int _currentBody;

  public GameObject[] mouth;
  private int _currentMouth;

  public GameObject[] eyes;
  private int _currentEyes;

  public GameObject[] nose;
  private int _currentNose;

  public GameObject[] wing;
  private int _currentWing;

  private static void ReRenderParts(IReadOnlyList<GameObject> parts, int index) {
    // Check if index is out of range
    if (index < 0 || index >= parts.Count) {
      throw new IndexOutOfRangeException();
    }

    foreach (var p in parts) {
      p.SetActive(false);
    }
    parts[index].SetActive(true);
  }

  private void ReRenderCharacter() {
    ReRenderParts(body, _currentBody);
    ReRenderParts(mouth, _currentMouth);
    ReRenderParts(eyes, _currentEyes);
    ReRenderParts(nose, _currentNose);
    ReRenderParts(wing, _currentWing);
  }

  public void SwitchBody() {
    _currentBody = (_currentBody + 1) % body.Length;
    ReRenderParts(body, _currentBody);
  }

  public void SwitchMouth() {
    _currentMouth = (_currentMouth + 1) % mouth.Length;
    ReRenderParts(mouth, _currentMouth);
  }
  public void SwitchEyes() {
    _currentEyes = (_currentEyes + 1) % eyes.Length;
    ReRenderParts(eyes, _currentEyes);
  }

  public void SwitchNose() {
    _currentNose = (_currentNose + 1) % nose.Length;
    ReRenderParts(nose, _currentNose);
  }

  public void SwitchWing() {
    _currentWing = (_currentWing + 1) % wing.Length;
    ReRenderParts(wing, _currentWing);
  }
  
  private void Awake() {
    ReRenderCharacter();
  }
}
