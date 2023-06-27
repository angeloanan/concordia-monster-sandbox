using UnityEngine;
using UnityEngine.UIElements;

public class CustomizationBox : VisualElement {
  public new class UxmlFactory : UxmlFactory<Button> { }

  public CustomizationBox() { }

  private Button BoxButton => this.Q<Button>("AttributeContainer");
  private VisualElement Attribute => this.Q<VisualElement>("Attribute");

  public CustomizationBox(Texture2D iconSprite, EventCallback<ClickEvent> onClick = null) {
    Init(iconSprite, onClick);
  }

  public void Init(Texture2D iconSprite, EventCallback<ClickEvent> onClick = null) {
    var asset = Resources.Load<VisualTreeAsset>("UI/Components/CustomizationBox");
    asset.CloneTree(this);

    if (iconSprite != null) Attribute.style.backgroundImage = new StyleBackground(iconSprite);
    if (onClick != null) this.RegisterCallback(onClick);
  }
}