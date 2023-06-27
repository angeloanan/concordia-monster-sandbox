namespace DataStore {
  public class ItemData {
    public class ItemEntry {
      /// <summary>
      /// Item name to be display
      /// 
      /// Probably won't be used, but it's here just in case
      /// </summary>
      public string itemLabel;

      /// <summary>
      /// Which prefab to spawn
      /// Path relative to `/Assets/Prefab/Resources/`
      /// </summary>
      public string prefabPath;

      /// <summary>
      /// The preview image for the item
      /// Path relative to `/Assets/Resources/Images`
      /// </summary>
      public string previewImagePath;

      /// <summary>
      /// SFX to play when the item is spawned
      /// Path relative to `/Assets/Resources/Audio/spawn`
      /// </summary>
      public string spawnSfxPath;
    }

    public class ItemGroup {
      public ItemEntry[] buildings;
      public ItemEntry[] trees;
      public ItemEntry[] decorations;
      public ItemEntry[] textBoxes;
    }

    public static readonly ItemGroup UIItemMap = new() {
      buildings = new ItemEntry[] {
        new() { itemLabel = "Arc", prefabPath = "Objects/arc", spawnSfxPath = "building/1" },
        new() { itemLabel = "Bench", prefabPath = "Objects/bench", spawnSfxPath = "building/2" },
        new() { itemLabel = "Chest", prefabPath = "Objects/chest", spawnSfxPath = "building/1" },
        new() { itemLabel = "Fence", prefabPath = "Objects/fence", spawnSfxPath = "building/3" },
        new() { itemLabel = "House", prefabPath = "Objects/house", spawnSfxPath = "building/1" },
      },
      trees = new ItemEntry[] {
        new() { itemLabel = "Pine Tree", prefabPath = "Objects/pinetree", spawnSfxPath = "tree/1" },
        new() { itemLabel = "Dead Tree", prefabPath = "Objects/treeDead", spawnSfxPath = "tree/2" },
      },
      decorations = new ItemEntry[] {
        new() { itemLabel = "Lantern", prefabPath = "Objects/lantern", spawnSfxPath = "decor/1" },
        new() { itemLabel = "Pumpkin", prefabPath = "Objects/pumpkin", spawnSfxPath = "decor/2" },
        new() { itemLabel = "Rock", prefabPath = "Objects/rock", spawnSfxPath = "decor/3" },
        new() { itemLabel = "Double Rock", prefabPath = "Objects/rockDouble", spawnSfxPath = "rocks/1" },
        new() { itemLabel = "Tall Rock", prefabPath = "Objects/rockTall", spawnSfxPath = "rocks/1" },
        new() { itemLabel = "Skull", prefabPath = "Objects/skull", spawnSfxPath = "decor/1" },
      },
      textBoxes = new ItemEntry[]
      {
        new() { itemLabel = "textbox", prefabPath = "", spawnSfxPath = "" },
      }
    };
  }
}