using System;
using System.Collections.Generic;
using System.Linq;
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

    // HACK
    GameObject[][] allAttributes = monsterName switch {
      "fairymonster" => new[] { body, hair, arms, wing, legs, ears },
      "devilmonster" => new[] { body, hair, arms, ears, wing, legs, eyes },
      "fluffymonster" => new[] { body, hair, arms, ears, legs, accessories, wing, eyes },
      "potatomonster" => new[] { hair, arms, ears, wing, legs },
      _ => throw new ArgumentException($"Unknown monster name {monsterName}")
    };

    // HACK
    if (monsterName == "potatomonster") {
      var transformList = MonsterDataManager.Instance.activeMonsterPrefab.transform
        .Find("Mouth")
        .gameObject
        .GetComponentsInChildren<Transform>();
      var activeBody = transformList[^1];
      Debug.Assert(activeBody != null, "activeBody != null");
      var activeBodyIndex = activeBody.name.ToCharArray()[activeBody.name.Length - 1] - '0';
      var bodyIndex = index % 4 + (4 * (activeBodyIndex - 1));
      Debug.Log(bodyIndex);
      index = bodyIndex;

      activeBody.GetComponent<Renderer>().material = materials[index];
    }

    var chosenMaterial = materials[index];
    
    foreach (var attributeGroup in allAttributes) {
      foreach (var part in attributeGroup) {
        if (part.TryGetComponent(out Renderer partRenderer)) {
          partRenderer.material = chosenMaterial;
        }

        // TODO: Recursive but lazy
        if (part.transform.childCount < 0) continue;
        for (int i = 0; i < part.transform.childCount; i++) {
          if (part.transform.GetChild(i).TryGetComponent(out Renderer subPartRenderer)) {
            Debug.Log(subPartRenderer.name);
            subPartRenderer.material = chosenMaterial;
          }
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