using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class CharacterCustomizationListEntry : VisualElement {
  public new class UxmlFactor : UxmlFactory<VisualElement> { }

  public CharacterCustomizationListEntry() { }

  private Button Button => this.Q<Button>("ActionButton");
  private Label Label => this.Q<Label>("Label");

  public CharacterCustomizationListEntry(string labelText = "", EventCallback<ClickEvent> onClick = null) {
    Init(labelText, onClick);
  }


  public void Init(string labelText = "", EventCallback<ClickEvent> onClick = null) {
    var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/CharacterCustomizationListEntry.uxml");
    asset.CloneTree(this);
    
    this.Label.text = labelText;
    // this.Label.BindProperty(property);

    if (onClick != null) {
      this.Button.RegisterCallback(onClick);
    }
  }
}