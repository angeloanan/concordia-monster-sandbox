using UnityEngine;
using UnityEngine.UIElements;

public class TextBubble : VisualElement {
  public new class UxmlFactory : UxmlFactory<VisualElement> { }

  public TextBubble() { }

  public VisualElement Container => this.Q<VisualElement>("Image");
  private Label TextContent => Container.Q<Label>("Text");

  public TextBubble(string text = "") {
    Init(text);
  }

  public void Init(string labelText = "") {
    var asset = Resources.Load<VisualTreeAsset>("UI/Components/TextBubble");
    asset.CloneTree(this);

    TextContent.text = labelText;
  }
}