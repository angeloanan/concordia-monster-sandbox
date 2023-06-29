using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CustomizeCharacterBehavior : MonoBehaviour {
  [SerializeField] private Vector3 screenCenter;
  [SerializeField] private UIDocument uiDocument;

  private CharacterCustomization _characterCustomization;

  private Label _categoryStep;
  private VisualElement _categoryContainer;
  private VisualElement _customizationContainer;

  public void RenderMonsterAttributesBox(int attributeIdx, string attributeGroup,
    List<MonsterCustomizationEntry> customizations) {
    _customizationContainer.Clear();
    _categoryStep.text =
      $"{attributeIdx}{_categoryStep.text.Substring(1, _categoryStep.text.Length - 1)}";

    var partIndex = 0;
    foreach (var monsterPart in customizations) {
      var image = Resources.Load<Texture2D>(monsterPart.IconPath);
      var i1 = partIndex;
      var box = new CustomizationBox(image,
        _ => {
          Debug.Log($"Rendering {attributeGroup} index {i1}");
          CharacterCustomization.ReRenderParts(_characterCustomization[attributeGroup], i1);
        });
      _customizationContainer.Add(box);
      partIndex++;
    }
  }

  public void RenderMonsterPaletteBox(int attributeIdx, string attributeGroup,
    List<MonsterCustomizationEntry> colors) {
    _customizationContainer.Clear();
    _categoryStep.text =
      $"{attributeIdx}{_categoryStep.text.Substring(1, _categoryStep.text.Length - 1)}";

    var partIndex = 0;
    foreach (var color in colors) {
      var image = Resources.Load<Texture2D>(color.IconPath);
      var i1 = partIndex;
      var box = new CustomizationBox(image,
        _ => {
          MonsterDataManager.Instance.activeMonsterPrefab.GetComponent<CharacterCustomization>()
            .ChangeMaterials(i1, MonsterDataManager.Instance.activeMonsterPrefab.name);
        });
      _customizationContainer.Add(box);
      partIndex++;
    }
  }

  public void Awake() {
    Debug.Log("Switched to CustomizeCharacter Scene");
    Debug.Assert(MonsterDataManager.Instance.activeMonsterPrefab != null,
      "MonsterDataManager.Instance.activeMonsterPrefab != null");
    var monster = MonsterDataManager.Instance.activeMonsterPrefab;

    _characterCustomization = monster.GetComponent<CharacterCustomization>();
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
    var step = 1;
    foreach (var customization in monsterData.customizations) {
      var image = Resources.Load<Texture2D>(
        $"Images/Icons/customization/{monster.name.ToLower().Substring(0, monster.name.Length - 7)}_{customization.Key.ToLower()}");

      var step1 = step;
      var box = new CustomizationBox(image,
        _ => {
          if (customization.Key.ToLower() == "palette") {
            RenderMonsterPaletteBox(step1, customization.Key, customization.Value);
          }
          else {
            RenderMonsterAttributesBox(step1, customization.Key, customization.Value);
          }
        });

      _categoryContainer.Add(box);
      step++;
    }

    // What to do:
    // 1. Render all body parts in monsterData.Customizations[0] on customizationContainer
    // 2. Loop over all monsterData.Customization, render icon and register callback on Category Container
    //   On callback:
    //     * Clear customizationContainer
    //     * Render all body parts in monsterData.Customizations[index]
    //     * Update categoryStep.text
    var firstKey = monsterData.customizations.Keys.First();
    var firstCustomization = monsterData.customizations[firstKey];
    RenderMonsterAttributesBox(1, firstKey, firstCustomization);

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