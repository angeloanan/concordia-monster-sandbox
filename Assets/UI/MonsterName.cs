using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MonsterName : MonoBehaviour {
  private void Awake() {
    var root = GetComponent<UIDocument>().rootVisualElement;
    var continueButton = root.Q<Button>("ContinueButton");
    var monsterNameTextFeld = root.Q<TextField>();

    continueButton.RegisterCallback<ClickEvent>(_ => {
      MonsterDataManager.Instance.SetCurrentActiveMonsterName(monsterNameTextFeld.value);

      SceneManager.LoadScene("Scenes/GameWorld");
    });
  }
}