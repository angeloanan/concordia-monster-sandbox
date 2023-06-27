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
      customizations = new Dictionary<string, List<MonsterCustomizationEntry>> {
        {
          "Mouth", new() {
            new() {
              Label = "Mouth 1",
              IconPath = "Images/Monsters/fluffy/Mouth/1",
            },
            new() {
              Label = "Mouth 2",
              IconPath = "Images/Monsters/fluffy/Mouth/2",
            },
            new() {
              Label = "Mouth 3",
              IconPath = "Images/Monsters/fluffy/Mouth/3",
            },
            new() {
              Label = "Mouth 4",
              IconPath = "Images/Monsters/fluffy/Mouth/4",
            },
            new() {
              Label = "Mouth 5",
              IconPath = "Images/Monsters/fluffy/Mouth/5",
            },
            new() {
              Label = "Mouth 6",
              IconPath = "Images/Monsters/fluffy/Mouth/6",
            },
          }
        },
        {
          "Eyes", new() {
            new() {
              Label = "Eyes 1",
              IconPath = "Images/Monsters/fluffy/Eyes/1",
            },
            new() {
              Label = "Eyes 2",
              IconPath = "Images/Monsters/fluffy/Eyes/2",
            },
            new() {
              Label = "Eyes 3",
              IconPath = "Images/Monsters/fluffy/Eyes/3",
            },
            new() {
              Label = "Eyes 4",
              IconPath = "Images/Monsters/fluffy/Eyes/4",
            },
            new() {
              Label = "Eyes 5",
              IconPath = "Images/Monsters/fluffy/Eyes/5",
            },
          }
        },
        {
          "Emotion", new() {
            new() {
              Label = "Emotion 1",
              IconPath = "Images/Monsters/fluffy/Emotion/1",
            },
            new() {
              Label = "Emotion 2",
              IconPath = "Images/Monsters/fluffy/Emotion/2",
            },
            new() {
              Label = "Emotion 3",
              IconPath = "Images/Monsters/fluffy/Emotion/3",
            },
          }
        },
        {
          "Arms", new() {
            new() {
              Label = "Arms 1",
              IconPath = "Images/Monsters/fluffy/Arms/1",
            },
            new() {
              Label = "Arms 2",
              IconPath = "Images/Monsters/fluffy/Arms/2",
            },
            new() {
              Label = "Arms 3",
              IconPath = "Images/Monsters/fluffy/Arms/3",
            },
          }
        }
      },
      sfxData = new MonsterSfxData {
        interaction = "interactions/monster/3",
        spawn = "spawn/monster/3",
      }
    },
    new() {
      name = "PotatoMonster",
      customizations = new Dictionary<string, List<MonsterCustomizationEntry>> {
        {
          "Eyes", new() {
            new() {
              Label = "Eyes 1",
              IconPath = "Images/Monsters/potato/Eyes/1",
            },
            new() {
              Label = "Eyes 2",
              IconPath = "Images/Monsters/potato/Eyes/2",
            },
            new() {
              Label = "Eyes 3",
              IconPath = "Images/Monsters/potato/Eyes/3",
            },
          }
        },
        {
          "Arms", new() {
            new() {
              Label = "Arms 1",
              IconPath = "Images/Monsters/potato/Arms/1",
            },
            new() {
              Label = "Arms 2",
              IconPath = "Images/Monsters/potato/Arms/2",
            },
            new() {
              Label = "Arms 3",
              IconPath = "Images/Monsters/potato/Arms/3",
            },
          }
        },
        {
          "Emotion", new() {
            new() {
              Label = "Emotion 1",
              IconPath = "Images/Monsters/potato/Emotion/1",
            },
            new() {
              Label = "Emotion 2",
              IconPath = "Images/Monsters/potato/Emotion/2",
            },
            new() {
              Label = "Emotion 3",
              IconPath = "Images/Monsters/potato/Emotion/3",
            },
          }
        },
      },
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
            new() {
              Label = "Eyes 4",
              IconPath = "Images/Monsters/devil/Eyes/4",
            },
          }
        },
        {
          "Mouth", new() {
            new() {
              Label = "Eyes 1",
              IconPath = "Images/Monsters/devil/Mouth/1",
            },
            new() {
              Label = "Eyes 2",
              IconPath = "Images/Monsters/devil/Mouth/2",
            },
            new() {
              Label = "Eyes 3",
              IconPath = "Images/Monsters/devil/Mouth/3",
            },
          }
        },
        {
          "Horn", new() {
            new() {
              Label = "Horn 1",
              IconPath = "Images/Monsters/devil/Horn/1",
            },
            new() {
              Label = "Horn 2",
              IconPath = "Images/Monsters/devil/Horn/2",
            },
          }
        },
        {
          "Emotion", new() {
            new() {
              Label = "Emotion 1",
              IconPath = "Images/Monsters/devil/Emotion/1",
            },
            new() {
              Label = "Emotion 2",
              IconPath = "Images/Monsters/devil/Emotion/2",
            },
            new() {
              Label = "Emotion 3",
              IconPath = "Images/Monsters/devil/Emotion/3",
            },
          }
        },
      },
      sfxData = new MonsterSfxData {
        interaction = "interactions/monster/1",
        spawn = "spawn/monster/1",
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