using UnityEditor;
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
    var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/Components/CharacterCustomizationListEntry.uxml");
    asset.CloneTree(this);
    
    this.Label.text = labelText;
    // this.Label.BindProperty(property);

    if (onClick != null) {
      this.Button.RegisterCallback(onClick);
    }
  }
}