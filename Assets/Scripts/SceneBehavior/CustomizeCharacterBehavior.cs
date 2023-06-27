using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CustomizeCharacterBehavior : MonoBehaviour {
  [SerializeField] private Vector3 screenCenter;
  [SerializeField] private UIDocument uiDocument;

  private Label _categoryStep;
  private VisualElement _categoryContainer;
  private VisualElement _customizationContainer;

  public void RenderMonsterAttributesBox(List<MonsterCustomizationEntry> customizations
    ) {
    _customizationContainer.Clear();

    foreach (var monsterPart in customizations) {
      var image = Resources.Load<Texture2D>(monsterPart.IconPath);
      var box = new CustomizationBox(image, _ => {
        // TODO: Actually handle callbacks
      });

      _customizationContainer.Add(box);
    }
  }

  public void Awake() {
    Debug.Log("Switched to CustomizeCharacter Scene");
    Debug.Assert(MonsterDataManager.Instance.activeMonsterPrefab != null,
      "MonsterDataManager.Instance.activeMonsterPrefab != null");
    var monster = MonsterDataManager.Instance.activeMonsterPrefab;

    monster.transform.position = screenCenter;

    // Initialize UI
    var root = uiDocument.rootVisualElement;
    _categoryStep = root.Q<Label>("CategoryStep");
    _categoryContainer = root.Q<VisualElement>("CategoryContainer");
    _customizationContainer = root.Q<VisualElement>("CustomizationContainer");

    // Get Monster Customization Data based on selected monster
    var monsterData = MonsterDataManager.ResolveMonsterData(monster.name);
    Debug.Assert(monsterData != null, nameof(monsterData) + " != null");
    var monsterCustomizationScript = monster.GetComponent<CharacterCustomization>();

    _categoryStep.text = $"1 / {monsterData.customizations.Count}";

    // Map over all customization attributes category and create UI and assign callbacks
    foreach (var customization in monsterData.customizations) {
      // TODO: Once attributes icon are here, use them
      // var image = Resources.Load<Texture2D>();
      var box = new CustomizationBox(null,
        _ => { RenderMonsterAttributesBox(customization.Value); });
      
      _categoryContainer.Add(box);
    }

    // What to do:
    // 1. Render all body parts in monsterData.Customizations[0] on customizationContainer
    // 2. Loop over all monsterData.Customization, render icon and register callback on Category Container
    //   On callback:
    //     * Clear customizationContainer
    //     * Render all body parts in monsterData.Customizations[index]
    //     * Update categoryStep.text
    var firstKey = monsterData.customizations.Keys.First();
    foreach (var monsterPart in monsterData.customizations[firstKey]) {
      var iconPath = Resources.Load<Texture2D>(monsterPart.IconPath);

      var box = new CustomizationBox(iconPath, evt => { monsterCustomizationScript.SwitchEyes(); });

      _customizationContainer.Add(box);
      Debug.Log($"Added CustomizationBox for {monster.name} ({monsterPart.IconPath})");
    }

    var doneButton = root.Q<Button>("DoneButton");
    doneButton.RegisterCallback<ClickEvent>(_ => { AudioManager.Instance.PlayUiClick(); });
    doneButton.RegisterCallback<ClickEvent>(_ => {
      Debug.Log("Done button clicked. Transitioning to GameWorld");

      // Update current monster reference
      MonsterDataManager.Instance.SetCurrentActiveMonster(monster);

      // Navigate to next scene
      SceneManager.LoadScene("Scenes/MonsterName");
    });
  }
}