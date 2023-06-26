using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterData {
  public string name;

  // Dictionary<MonsterPart, MonsterCustomizationEntry[]>
  public Dictionary<string, List<MonsterCustomizationEntry>> customizations;

  public MonsterSfxData sfxData;
}

public class MonsterCustomizationEntry {
  public string Label;
  public string IconPath;
}

/// <summary>
/// String location relative to `/Assets/Resources/Audio/` to the audio clip
/// </summary>
public class MonsterSfxData {
  public string spawn;
  public string interaction;
}

public class MonsterDataManager : MonoBehaviour {
  public static MonsterDataManager Instance { get; private set; }

  public string activeMonsterName;
  public GameObject activeMonsterPrefab;

  public static readonly MonsterData[] MonsterData = {
    new() {
      name = "FairyMonster",
      customizations = new Dictionary<string, List<MonsterCustomizationEntry>> { },
      sfxData = new MonsterSfxData {
        interaction = "interactions/monster/4",
        spawn = "spawn/monster/4",
      }
    },
    new() {
      name = "FluffyMonster",
      customizations = new Dictionary<string, List<MonsterCustomizationEntry>> { },
      sfxData = new MonsterSfxData {
        interaction = "interactions/monster/3",
        spawn = "spawn/monster/3",
      }
    },
    new() {
      name = "PotatoMonster",
      customizations = new Dictionary<string, List<MonsterCustomizationEntry>> { },
      sfxData = new MonsterSfxData {
        interaction = "interactions/monster/2",
        spawn = "spawn/monster/2",
      }
    },
    new() {
      name = "DevilMonster",
      customizations = new Dictionary<string, List<MonsterCustomizationEntry>> {
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
          "eyes", new() {
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
          "nose", new() {
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
          "wing", new() {
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
      },
      sfxData = new MonsterSfxData {
        interaction = "interactions/monster/1",
        spawn = "spawn/monster/1",
        }*/
      }
    },
  };

  public static MonsterData ResolveMonsterData(string monsterName) {
    return MonsterData.FirstOrDefault(data => data.name == monsterName);
  }

  public void SetCurrentActiveMonster(GameObject monster) {
    Debug.Log($"Setting active monster to {monster.name}");
    activeMonsterPrefab = monster;
  }


  public void SetCurrentActiveMonsterName(string monsterName) {
    Debug.Log($"Setting active monster name to {monsterName}");
    activeMonsterName = monsterName;
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