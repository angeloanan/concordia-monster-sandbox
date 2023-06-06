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
    var doneButton = root.Q<Button>("DoneButton");
    
    doneButton.RegisterCallback<ClickEvent>(_ => {
      Debug.Log("Done button clicked");
      // Update current monster reference
      MonsterDataManager.Instance.SetCurrentActiveMonster(monster);
      
      // Navigate to next scene
      SceneManager.LoadScene("PlaytestTwo");
    });
  }
}