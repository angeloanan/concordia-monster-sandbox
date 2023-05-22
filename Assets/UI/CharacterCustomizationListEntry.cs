using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class CharacterCustomizationListEntry : VisualElement {
  public new class UxmlFactor : UxmlFactory<VisualElement> { }

  public CharacterCustomizationListEntry() { }

  private Button Button => this.Q<Button>("ActionButton");
  private Label label => this.Q<Label>("Label");

  public CharacterCustomizationListEntry(SerializedProperty property, string labelText = "") {
    Init(property, labelText);
  }


  public void Init(SerializedProperty property, string labelText = "") {
    var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/CharacterCustomizationListEntry.uxml");
    asset.CloneTree(this);
    
    this.label.text = labelText;
    this.label.BindProperty(property);
  }
}