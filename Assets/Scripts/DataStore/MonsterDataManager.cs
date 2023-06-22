using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterData {
  public string Name;

  // Dictionary<MonsterPart, MonsterCustomizationEntry[]>
  public Dictionary<string, List<MonsterCustomizationEntry>> Customizations;
}

public class MonsterCustomizationEntry {
  public string Label;
  public string IconPath;
}

public class MonsterDataManager : MonoBehaviour {
  public static MonsterDataManager Instance { get; private set; }

  public GameObject activeMonsterPrefab;

  public static readonly MonsterData[] MonsterData = {
    new() {
      Name = "DevilMonster",
      Customizations = new Dictionary<string, List<MonsterCustomizationEntry>> {
        {
          "Eyes", new() {
            new() {
              Label = "Eyes 1",
              IconPath = "Images/Monsters/devil/Eyes/1",
            },
            new() {
              Label = "Eyes 2",
              IconPath = "Images/Monsters/devil/Eyes/2",
            },
            new() {
              Label = "Eyes 3",
              IconPath = "Images/Monsters/devil/Eyes/3",
            },
          }
        }, 
        /*{
          "mouth", new() {
            new() {
              Label = "Mouth 1",
              Icon = "mouth1",
            },
            new() {
              Label = "Mouth 2",
              Icon = "mouth2",
            },
            new() {
              Label = "Mouth 3",
              Icon = "mouth3",
            },
          }
        }, {
          "eyes", new() {
            new() {
              Label = "Eyes 1",
              Icon = "eyes1",
            },
            new() {
              Label = "Eyes 2",
              Icon = "eyes2",
            },
            new() {
              Label = "Eyes 3",
              Icon = "eyes3",
            },
          }
        }, {
          "nose", new() {
            new() {
              Label = "Nose 1",
              Icon = "nose1",
            },
            new() {
              Label = "Nose 2",
              Icon = "nose2",
            },
            new() {
              Label = "Nose 3",
              Icon = "nose3",
            },
          }
        }, {
          "wing", new() {
            new() {
              Label = "Wing 1",
              Icon = "wing1",
            },
            new() {
              Label = "Wing 2",
              Icon = "wing2",
            },
            new() {
              Label = "Wing 3",
              Icon = "wing3",
            },
          }
        }*/
      }
    }
  };

  public static MonsterData ResolveMonsterData(string monsterName) {
    return MonsterData.FirstOrDefault(data => data.Name == monsterName);
  }

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