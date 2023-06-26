using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

// Want to use State Machines but not sure how to do it in Unity
public enum MonsterSelectBehaviorState {
  CharacterSelection,
  CharacterConfirmation,
}

public class MonsterSelectBehavior : MonoBehaviour {
  [SerializeField] private List<GameObject> characters = new();
  [SerializeField] private UIDocument uiDocument;

  private Vector2 _cursorPosition;
  private GameObject _selectedMonster;
  private MonsterSelectBehaviorState _state = MonsterSelectBehaviorState.CharacterSelection;

  // Runs every time the mouse / input moves
  public void OnPointerMovement(InputAction.CallbackContext ctx) {
    _cursorPosition = ctx.ReadValue<Vector2>();
  }

  public void OnSelect(InputAction.CallbackContext ctx) {
    Debug.Assert(Camera.main != null, "Camera.main != null");

    if (ctx.performed) return;
    if (ctx.canceled) return;

    // Disable the whole thing if we're not in the right state
    if (_state == MonsterSelectBehaviorState.CharacterConfirmation) return;

    // Raytrace cursor position to world and check if it hits anything
    // If it hits something, then set the object to be dragged
    var ray = Camera.main.ScreenPointToRay(_cursorPosition);
    Physics.Raycast(ray, out var hit, 100);
    if (hit.collider == null) {
      Debug.Log("Selected nothing");
      return;
    }

    var character = hit.transform.gameObject;
    var characterInitialPosition = character.transform.position;
    if (!characters.Contains(character)) {
      Debug.Log($"Selected {character} but it is not a character");
      return;
    }

    Debug.Log($"Selected {character.name}");
    _state = MonsterSelectBehaviorState.CharacterConfirmation;

    var monsterData = MonsterDataManager.ResolveMonsterData(character.name);
    AudioManager.Instance.PlayAudio(monsterData?.sfxData?.interaction);

    // Despawn other character
    foreach (var c in characters) {
      if (c == character) continue;
      c.SetActive(false);
    }

    // Center & Enlarge selected character
    _selectedMonster = character;
    var screenCenter = new Vector3(0, -1.5f, -1);
    _selectedMonster.transform.position = screenCenter;
    _selectedMonster.transform.localScale = new Vector3(2, 2, 2); // 2x

    // Replace UI to confirm selection
    var root = uiDocument.rootVisualElement;
    // var characterSelectionContainer = root.Q<VisualElement>("CharacterSelectionContainer");
    // characterSelectionContainer.style.display = DisplayStyle.None;

    var confirmChooseAMonsterUiDoc = Resources.Load<VisualTreeAsset>("UI/ConfirmChooseAMonster");
    root.Clear();
    confirmChooseAMonsterUiDoc.CloneTree(root);

    // Add event listener to confirm button
    var confirmButton = root.Q<Button>("ConfirmButton");
    confirmButton.RegisterCallback<ClickEvent>(_ => { AudioManager.Instance.PlayUiClick(); });
    confirmButton.RegisterCallback<ClickEvent>(_ => {
      Debug.Log("Confirm button clicked");
      MonsterDataManager.Instance.SetCurrentActiveMonster(_selectedMonster);
      DontDestroyOnLoad(_selectedMonster);
      SceneManager.LoadScene("Scenes/CustomizeMonster");
    });

    var cancelButton = root.Q<Button>("CancelButton");
    cancelButton.RegisterCallback<ClickEvent>(_ => { AudioManager.Instance.PlayUiClick(); });
    cancelButton.RegisterCallback<ClickEvent>(_ => {
      Debug.Log("Cancel button clicked");
      _state = MonsterSelectBehaviorState.CharacterSelection;

      // Respawn other character
      foreach (var c in characters) {
        if (c == character) continue;
        c.SetActive(true);
      }

      // Center & Enlarge selected character
      character.transform.position = characterInitialPosition;
      character.transform.localScale = new Vector3(1, 1, 1); // 1x

      // Replace UI to confirm selection
      root.Clear();
      var chooseAMonsterUiDoc = Resources.Load<VisualTreeAsset>("UI/ChooseAMonster");
      chooseAMonsterUiDoc.CloneTree(root);
    });
  }
}