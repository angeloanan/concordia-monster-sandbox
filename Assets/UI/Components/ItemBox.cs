using UnityEditor;
using UnityEngine.UIElements;

public class ItemBox : VisualElement {
  public new class UxmlFactory : UxmlFactory<VisualElement> { }

  public ItemBox() { }
  
  private VisualElement Container => this.Q<VisualElement>("ItemContainer");
  private IMGUIContainer Icon => Container.Q<IMGUIContainer>("ItemIcon");
  private Label ItemBoxLabel => Container.Q<Label>("ItemLabel");

  public ItemBox(string labelText = "", EventCallback<ClickEvent> onClick = null) {
    Init(labelText, onClick);
  }

  public void Init(string labelText = "", EventCallback<ClickEvent> onClick = null) {
    var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/Components/ItemBox.uxml");
    asset.CloneTree(this);

    // Print the type of ItemBoxLabel
    ItemBoxLabel.text = labelText;
    if (onClick != null) {
      this.RegisterCallback(onClick);
    }
  }
}