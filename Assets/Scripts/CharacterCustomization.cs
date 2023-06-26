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

  public GameObject[] emotion;
  private int _currentEmotion;

  public GameObject[] hair;
  private int _currentHair;
  
  public GameObject[] arms;
  private int _currentArms;
  
  public GameObject[] legs;
  private int _currentLegs;
  
  public GameObject[] ears;
  private int _currentEars;
  
  public GameObject[] accessories;
  private int _currentAccessories;
  
  public GameObject[] wing;
  private int _currentWing;

  private static void ReRenderParts(IReadOnlyList<GameObject> parts, int index) {
    if (parts.Count == 0) return; // Whatever
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
    ReRenderParts(emotion, _currentEmotion);
    ReRenderParts(hair, _currentHair);
    ReRenderParts(arms, _currentArms);
    ReRenderParts(ears, _currentEars);
    ReRenderParts(accessories, _currentAccessories);
    ReRenderParts(wing, _currentWing);
    ReRenderParts(legs, _currentLegs);
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

  public void SwitchEmotion() {
    _currentEmotion = (_currentEmotion + 1) % emotion.Length;
    ReRenderParts(emotion, _currentEmotion);
  }

  public void SwitchHair() {
    _currentHair = (_currentHair + 1) % hair.Length;
    ReRenderParts(hair, _currentHair);
  }
  public void SwitchArms() {
    _currentArms = (_currentArms + 1) % arms.Length;
    ReRenderParts(arms, _currentArms);
  }
  
  public void SwitchEars() {
    _currentEars = (_currentEars + 1) % ears.Length;
    ReRenderParts(ears, _currentEars);
  }
  
  public void SwitchAccessories() {
    _currentAccessories = (_currentAccessories + 1) % accessories.Length;
    ReRenderParts(accessories, _currentAccessories);
  }
  
  public void SwitchWing() {
    _currentWing = (_currentWing + 1) % wing.Length;
    ReRenderParts(wing, _currentWing);
  }

  private void Awake() {
    ReRenderCharacter();
  }
}