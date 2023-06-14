using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CustomizeCharacterBehavior : MonoBehaviour {
  [SerializeField] private Vector3 screenCenter;
  [SerializeField] private UIDocument uiDocument;

  public void Awake() {
    Debug.Log("Switched to CustomizeCharacter Scene");
    Debug.Assert(MonsterDataManager.Instance.activeMonsterPrefab != null,
      "MonsterDataManager.Instance.activeMonsterPrefab != null");
    var monster = MonsterDataManager.Instance.activeMonsterPrefab;

    monster.transform.position = screenCenter;

    // Initialize UI
    var root = uiDocument.rootVisualElement;

    // Get Monster Customization Data based on selected monster
    var monsterData = MonsterDataManager.ResolveMonsterData(monster.name);
    Debug.Assert(monsterData != null, nameof(monsterData) + " != null");
    var monsterCustomizationScript = monster.GetComponent<CharacterCustomization>();

    var categoryStep = root.Q<Label>("CategoryStep");
    var categoryContainer = root.Q<VisualElement>("CategoryContainer");
    var customizationContainer = root.Q<VisualElement>("CustomizationContainer");

    categoryStep.text = $"1 / {monsterData.Customizations.Count}";

    // What to do:
    // 1. Render all body parts in monsterData.Customizations[0] on customizationContainer
    // 2. Loop over all monsterData.Customization, render icon and register callback on Category Container
    //   On callback:
    //     * Clear customizationContainer
    //     * Render all body parts in monsterData.Customizations[index]
    //     * Update categoryStep.text
    
    // Map over all customization data and create UI elements for each and assign callbacks
    foreach (var customization in monsterData.Customizations) {
      var customizationName = customization.Key;

      // Used for passing the index of monster part to the callback
      var monsterPartIndex = 0;
      foreach (var monsterPart in customization.Value) {
        var box = new CustomizationBox(monsterPart.Icon, evt =>
        {
              monsterCustomizationScript.SwitchEyes();
              
        });
        customizationContainer.Add(box);
        monsterPartIndex++;
      }
    }


    var doneButton = root.Q<Button>("DoneButton");
    doneButton.RegisterCallback<ClickEvent>(_ => {
      Debug.Log("Done button clicked. Transitioning to GameWorld");
      
      // Update current monster reference
      MonsterDataManager.Instance.SetCurrentActiveMonster(monster);

      // Navigate to next scene
      SceneManager.LoadScene("Scenes/MonsterName");
    });
  }
}