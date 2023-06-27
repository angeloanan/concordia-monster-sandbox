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
      customizations = new Dictionary<string, List<MonsterCustomizationEntry>> {
        {
          "Ears", new() {
            new() {
              Label = "Ears 1",
              IconPath = "Images/Monsters/fairy/Ears/1",
            },
            new() {
              Label = "Ears 2",
              IconPath = "Images/Monsters/fairy/Ears/2",
            },
            new() {
              Label = "Ears 3",
              IconPath = "Images/Monsters/fairy/Ears/3",
            },
          }
        },
        /*{
          "Emotion", new() {
            new() {
              Label = "Emotion 1",
              IconPath = "Images/Monsters/fairy/Emotion/1",
            },
            new() {
              Label = "Emotion 2",
              IconPath = "Images/Monsters/fairy/Emotion/2",
            },
            new() {
              Label = "Emotion 3",
              IconPath = "Images/Monsters/fairy/Emotion/3",
            },
          }
        },*/
        {
          "Eyes", new() {
            new() {
              Label = "Eyes 1",
              IconPath = "Images/Monsters/fairy/Eyes/1",
            },
            new() {
              Label = "Eyes 2",
              IconPath = "Images/Monsters/fairy/Eyes/2",
            },
            new() {
              Label = "Eyes 3",
              IconPath = "Images/Monsters/fairy/Eyes/3",
            },
          }
        },
        {
          "Hair", new() {
            new() {
              Label = "Hair 1",
              IconPath = "Images/Monsters/fairy/Hair/1",
            },
            new() {
              Label = "Hair 2",
              IconPath = "Images/Monsters/fairy/Hair/2",
            },
            new() {
              Label = "Hair 3",
              IconPath = "Images/Monsters/fairy/Hair/3",
            },
          }
        },
        {
          "Mouth", new() {
            new() {
              Label = "Mouth 1",
              IconPath = "Images/Monsters/fairy/Mouth/2",
            },
            new() {
              Label = "Mouth 2",
              IconPath = "Images/Monsters/fairy/Mouth/1",
            },
          }
        },
        {
          "Accessories", new() {
            new() {
              Label = "Necklace 1",
              IconPath = "Images/Monsters/fairy/Necklace/2",
            },
            new() {
              Label = "Necklace 2",
              IconPath = "Images/Monsters/fairy/Necklace/1",
            },
          }
        },
        {
          "Wing", new() {
            new() {
              Label = "Wing 1",
              IconPath = "Images/Monsters/fairy/Wing/1",
            },
            new() {
              Label = "Wing 2",
              IconPath = "Images/Monsters/fairy/Wing/2",
            },
            new() {
              Label = "Wing 3",
              IconPath = "Images/Monsters/fairy/Wing/3",
            },
          }
        },
      },
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
              IconPath = "Images/Monsters/fluffy/Eyes/4",
            },
            new() {
              Label = "Eyes 3",
              IconPath = "Images/Monsters/fluffy/Eyes/5",
            },
            new() {
              Label = "Eyes 4",
              IconPath = "Images/Monsters/fluffy/Eyes/2",
            },
            new() {
              Label = "Eyes 5",
              IconPath = "Images/Monsters/fluffy/Eyes/3",
            },
          }
        },
        /*{
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
        },*/
        {
          "Arms", new() {
            new() { 
              Label = "Arms 1",
              IconPath = "Images/Monsters/fluffy/Arms/3",
            },
            new() {
              Label = "Arms 2",
              IconPath = "Images/Monsters/fluffy/Arms/2",
            },
            new() {
              Label = "Arms 3",
              IconPath = "Images/Monsters/fluffy/Arms/1",
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
        /*{
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
        },*/
        {
          "Hair", new() {
            new() {
              Label = "Hair 1",
              IconPath = "Images/Monsters/potato/Hair/1",
            },
            new() {
              Label = "Hair 2",
              IconPath = "Images/Monsters/potato/Hair/2",
            },
            new() {
              Label = "Hair 3",
              IconPath = "Images/Monsters/potato/Hair/3",
            },
          }
        },
        {
          "Legs", new() {
            new() {
              Label = "Legs 1",
              IconPath = "Images/Monsters/potato/Legs/1",
            },
            new() {
              Label = "Legs 2",
              IconPath = "Images/Monsters/potato/Legs/2",
            },
          }
        },
        {
          "Mouth", new() {
            new() {
              Label = "Mouth 1",
              IconPath = "Images/Monsters/potato/Mouth/2",
            },
            new() {
              Label = "Mouth 2",
              IconPath = "Images/Monsters/potato/Mouth/1",
            },
            new() {
              Label = "Mouth 3",
              IconPath = "Images/Monsters/potato/Mouth/3",
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
              Label = "Mouth 1",
              IconPath = "Images/Monsters/devil/Mouth/1",
            },
            new() {
              Label = "Mouth 2",
              IconPath = "Images/Monsters/devil/Mouth/2",
            },
            new() {
              Label = "Mouth 3",
              IconPath = "Images/Monsters/devil/Mouth/3",
            },
          }
        },
        {
          "Accessories", new() {
            new() {
              Label = "Horn 1",
              IconPath = "Images/Monsters/devil/Horn/1",
            },
            new() {
              Label = "Horn 2",
              IconPath = "Images/Monsters/devil/Horn/2",
            },
            new() {
              Label = "Horn 3",
              IconPath = "Images/Monsters/devil/Horn/3",
            },
            new() {
              Label = "Horn 4",
              IconPath = "Images/Monsters/devil/Horn/4",
            },
            new() {
              Label = "Horn 5",
              IconPath = "Images/Monsters/devil/Horn/5",
            },
          }
        },
        /*{
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
        },*/
        {
          "Legs", new() {
            new() {
              Label = "Legs 1",
              IconPath = "Images/Monsters/devil/Legs/1",
            },
            new() {
              Label = "Legs 2",
              IconPath = "Images/Monsters/devil/Legs/2",
            }
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