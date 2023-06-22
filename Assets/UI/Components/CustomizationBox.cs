using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomizationBox : VisualElement
{
  public new class UxmlFactory : UxmlFactory<Button> { }

  public CustomizationBox() { }

  private Button BoxButton => this.Q<Button>("AttributeContainer");
  private VisualElement Attribute => this.Q<VisualElement>("Attribute");

  public CustomizationBox(Sprite iconSprite, EventCallback<ClickEvent> onClick = null)
  {
    Init(iconSprite, onClick);
  }

  public void Init(Sprite iconSprite, EventCallback<ClickEvent> onClick = null)
  {
    var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/Components/CustomizationBox.uxml");
    asset.CloneTree(this);

    if (iconSprite != null)
    {
      Attribute.style.backgroundImage = new StyleBackground(iconSprite.texture);
    }
    if (onClick != null)
    {
      this.RegisterCallback(onClick);
    }
  }
}
