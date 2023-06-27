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

  public void RenderMonsterAttributesBox(string part, List<MonsterCustomizationEntry> customizations) {
    _customizationContainer.Clear();

    var partIndex = 0;
    foreach (var monsterPart in customizations) {
      var image = Resources.Load<Texture2D>(monsterPart.IconPath);
      var i1 = partIndex;
      var box = new CustomizationBox(image,
        _ => {
          Debug.Log($"Rendering {part} index {i1}");
          // BUG: This doesn't work
          CharacterCustomization.ReRenderParts(_characterCustomization[part], i1);
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
    foreach (var customization in monsterData.customizations) {
      // TODO: Once attributes icon are here, use them
      var image = Resources.Load<Texture2D>($"Images/Icons/customization/{monster.name.ToLower().Substring(0, monster.name.Length - 7)}_{customization.Key.ToLower()}");
      Debug.Log(monster.name.ToLower().Substring(0, monster.name.Length - 7));
      Debug.Log(customization.Key.ToLower());
      
      var box = new CustomizationBox(image,
        _ => { RenderMonsterAttributesBox(customization.Key, customization.Value); });
  
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
    var firstCustomization = monsterData.customizations[firstKey];
    RenderMonsterAttributesBox(firstKey, firstCustomization);

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