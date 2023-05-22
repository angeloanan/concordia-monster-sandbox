using UnityEngine;
using UnityEngine.UIElements;

public class CustomizationMap {
  public string Label;
  public EventCallback<ClickEvent> OnClick;
}

public class CharacterCustomizationHUDBehavior : MonoBehaviour {
  [SerializeField] private CharacterCustomization _CharacterCustomization;

  private void OnEnable() {
    var characterCustomizationMap = new CustomizationMap[] {
      new() { Label = "Body Type", OnClick = _ => { _CharacterCustomization.SwitchBody(); } },
      new() { Label = "Eyes", OnClick = _ => { _CharacterCustomization.SwitchEyes(); } },
      new() { Label = "Mouth", OnClick = _ => { _CharacterCustomization.SwitchMouth(); } },
      new() { Label = "Ears", OnClick = _ => { Debug.Log("Unimplemented"); } },
      new() { Label = "Horns", OnClick = _ => { Debug.Log("Unimplemented"); } },
      new() { Label = "Wings", OnClick = _ => { _CharacterCustomization.SwitchWing(); } },
      new() { Label = "Tail", OnClick = _ => { Debug.Log("Unimplemented"); } },
      new() { Label = "Fur", OnClick = _ => { Debug.Log("Unimplemented"); } },
      new() { Label = "Color", OnClick = _ => { Debug.Log("Unimplemented"); } },
    };

    var root = GetComponent<UIDocument>().rootVisualElement;
    var customizationWindow = root.Q<VisualElement>("CustomizationWindow");
    var customizationWindowContent = customizationWindow.Q<VisualElement>("ContentContainer");

    foreach (CustomizationMap type in characterCustomizationMap) {
      var entry = new CharacterCustomizationListEntry(type.Label, type.OnClick);
      customizationWindowContent.Add(entry);
    }
  }
}