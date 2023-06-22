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
      /// Path relative to `/Assets/Resources/Images/Icons`
      /// </summary>
      public string previewImagePath;
    }

    public class ItemGroup {
      public ItemEntry[] buildings;
      public ItemEntry[] trees;
      public ItemEntry[] decorations;
      public ItemEntry[] textBoxes;
    }

    public static readonly ItemGroup UIItemMap = new() {
      buildings = new ItemEntry[] {
        new() { itemLabel = "Arc", prefabPath = "Objects/arc" },
        new() { itemLabel = "Bench", prefabPath = "Objects/bench" },
        new() { itemLabel = "Chest", prefabPath = "Objects/chest" },
        new() { itemLabel = "Fence", prefabPath = "Objects/fence" },
        new() { itemLabel = "House", prefabPath = "Objects/house" },
      },
      trees = new ItemEntry[] {
        new() { itemLabel = "Pine Tree", prefabPath = "Objects/pinetree" },
        new() { itemLabel = "Dead Tree", prefabPath = "Objects/treeDead" },
      },
      decorations = new ItemEntry[] {
        new() { itemLabel = "Lantern", prefabPath = "Objects/lantern" },
        new() { itemLabel = "Pumpkin", prefabPath = "Objects/pumpkin" },
        new() { itemLabel = "Rock", prefabPath = "Objects/rock" },
        new() { itemLabel = "Double Rock", prefabPath = "Objects/rockDouble" },
        new() { itemLabel = "Tall Rock", prefabPath = "Objects/rockTall" },
        new() { itemLabel = "Skull", prefabPath = "Objects/skull" },
      },
      textBoxes = new ItemEntry[] { }
    };

  }
}