using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CustomizeCharacterBehavior : MonoBehaviour { 
  [SerializeField] private Vector3 screenCenter;
  [SerializeField] private UIDocument uiDocument;
  
  public void Awake() {
    Debug.Log("Switched to CustomizeCharacter Scene");
    Debug.Assert(MonsterDataManager.Instance.activeMonsterPrefab != null, "MonsterDataManager.Instance.activeMonsterPrefab != null");
    var monster = MonsterDataManager.Instance.activeMonsterPrefab;

    monster.transform.position = screenCenter;
    
    // Initialize UI
    var root = uiDocument.rootVisualElement;
    
    // Get Monster Customization Data based on selected monster
    var monsterData = MonsterDataManager.ResolveMonsterData(monster.name);
    Debug.Assert(monsterData != null, nameof(monsterData) + " != null");
    
    // Map over all customization data and create UI elements for each and assign callbacks
    foreach (var customization in monsterData.Customizations) {
      
    }
    
    
    var doneButton = root.Q<Button>("DoneButton");
    doneButton.RegisterCallback<ClickEvent>(_ => {
      Debug.Log("Done button clicked");
      // Update current monster reference
      MonsterDataManager.Instance.SetCurrentActiveMonster(monster);
      
      // Navigate to next scene
      SceneManager.LoadScene("Scenes/GameWorld");
    });
  }
}