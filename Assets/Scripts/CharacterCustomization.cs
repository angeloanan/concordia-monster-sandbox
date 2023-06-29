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

  public GameObject[] colors;
  private int _currentColor;

  public Material[] materials;
  
  private Renderer rend;

  public static void ReRenderParts(IReadOnlyList<GameObject> parts, int index) {
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

  public void ChangeMaterials(int index, string monsterName) {
    monsterName = monsterName.ToLower();
    GameObject[][] allAttributes = {};

    switch (monsterName) {
      case "fairymonster": {
        allAttributes = new []{body, hair, arms, wing, legs, ears};
        break;
      }
      case "devilmonster": {
        allAttributes = new []{body, hair, arms, ears, wing, legs};
        break;
      }
      case "fluffymonster": {
        allAttributes = new []{body, hair, arms, ears, legs, accessories, wing};
        break;
      }
      case "potatomonster": {
        allAttributes = new []{body, hair, arms, ears, wing, legs};
        break;
      }
      default:
        throw new ArgumentException($"Unknown monster name {monsterName}");
    }
    
    var chosenMaterial = materials[index];
    
    foreach (var attributeGroup in allAttributes) {
      foreach (var part in attributeGroup) {
        if (part.TryGetComponent(out Renderer partRenderer)) {
          partRenderer.material = chosenMaterial;
        }
      }
    }
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

  private void Awake() {
    ReRenderCharacter();
  }

  public IReadOnlyList<GameObject> this[string part] {
    get {
      switch (part.ToLower()) {
        case "body":
          return body;
        case "mouth":
          return mouth;
        case "eyes":
          return eyes;
        case "emotion":
          return emotion;
        case "hair":
          return hair;
        case "arms":
          return arms;
        case "ears":
          return ears;
        case "accessories":
          return accessories;
        case "wing":
          return wing;
        case "legs":
          return legs;
        default:
          return null;
      }
    }
  }
}