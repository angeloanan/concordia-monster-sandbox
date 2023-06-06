using System.Collections.Generic;
using UnityEngine;

public class MonsterData {
  public string name;
  // Dictionary<MonsterPart, MonsterCustomizationEntry[]>
  public Dictionary<string, List<MonsterCustomizationEntry>> customizations;
}

public class MonsterCustomizationEntry {
  public string label;
  public string icon;
}

public class MonsterDataManager : MonoBehaviour {
  public static MonsterDataManager Instance { get; private set; }

  public GameObject activeMonsterPrefab;

  public static MonsterData[] MonsterData = {
    new() {
      name = "Monster 1",
      customizations = new Dictionary<string, List<MonsterCustomizationEntry>> {
        {
          "body", new List<MonsterCustomizationEntry> {
            new() {
              label = "Body 1",
              icon = "body1",
            },
            new() {
              label = "Body 2",
              icon = "body2",
            },
            new() {
              label = "Body 3",
              icon = "body3",
            },
          }
        }, {
          "mouth", new List<MonsterCustomizationEntry> {
            new() {
              label = "Mouth 1",
              icon = "mouth1",
            },
            new() {
              label = "Mouth 2",
              icon = "mouth2",
            },
            new() {
              label = "Mouth 3",
              icon = "mouth3",
            },
          }
        }, {
          "eyes", new List<MonsterCustomizationEntry> {
            new() {
              label = "Eyes 1",
              icon = "eyes1",
            },
            new() {
              label = "Eyes 2",
              icon = "eyes2",
            },
            new() {
              label = "Eyes 3",
              icon = "eyes3",
            },
          }
        }, {
          "nose", new List<MonsterCustomizationEntry> {
            new() {
              label = "Nose 1",
              icon = "nose1",
            },
            new() {
              label = "Nose 2",
              icon = "nose2",
            },
            new() {
              label = "Nose 3",
              icon = "nose3",
            },
          }
        }, {
          "wing", new List<MonsterCustomizationEntry> {
            new() {
              label = "Wing 1",
              icon = "wing1",
            },
            new() {
              label = "Wing 2",
              icon = "wing2",
            },
            new() {
              label = "Wing 3",
              icon = "wing3",
            },
          }
        }
      }
    }
  };

  public void SetCurrentActiveMonster(GameObject monster) {
    Debug.Log($"Setting active monster to {monster.name}");
    activeMonsterPrefab = monster;
  }

  private void Awake() {
    if (Instance == null) {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else {
      Destroy(gameObject);
    }
  }
}