using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterCustomizationListEntry : VisualElement {
  public new class UxmlFactory : UxmlFactory<VisualElement> { }

  public CharacterCustomizationListEntry() { }

  private Button Button => this.Q<Button>("ActionButton");
  private Label Label => this.Q<Label>("Label");

  public CharacterCustomizationListEntry(string labelText = "", EventCallback<ClickEvent> onClick = null) {
    Init(labelText, onClick);
  }

  public void Init(string labelText = "", EventCallback<ClickEvent> onClick = null) {
    var asset = Resources.Load<VisualTreeAsset>("UI/Components/CharacterCustomizationListEntry");
    asset.CloneTree(this);
    
    this.Label.text = labelText;
    // this.Label.BindProperty(property);

    if (onClick != null) {
      this.Button.RegisterCallback(onClick);
    }
  }
}