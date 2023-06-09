using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MonsterName : MonoBehaviour {
  private TextField monsterNameField;
  private TouchScreenKeyboard _kb;
  private void Awake() {
    var root = GetComponent<UIDocument>().rootVisualElement;
    var continueButton = root.Q<Button>("ContinueButton");
    monsterNameField = root.Q<TextField>();

    continueButton.RegisterCallback<ClickEvent>(_ => { AudioManager.Instance.PlayUiClick(); });
    continueButton.RegisterCallback<ClickEvent>(_ => {
      MonsterDataManager.Instance.SetCurrentActiveMonsterName(monsterNameField.value);

      SceneManager.LoadScene("Scenes/GameWorld");
    });
    
    monsterNameField.Focus();
  }
}