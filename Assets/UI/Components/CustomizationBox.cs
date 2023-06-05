using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomizationBox : VisualElement {
  public new class UxmlFactory : UxmlFactory<Button> { }

  public CustomizationBox() { }
  private Button BoxButton => this.Q<Button>();

  public CustomizationBox(string imagePath, EventCallback<ClickEvent> onClick = null) {
    Init(imagePath, onClick);
  }

  public void Init(string imagePath, EventCallback<ClickEvent> onClick = null) {
    var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/Components/CustomizationBox.uxml");
    asset.CloneTree(this);

    if (imagePath != null) {
      BoxButton.style.backgroundImage = new StyleBackground(AssetDatabase.LoadAssetAtPath<Texture2D>(imagePath));
    }
    if (onClick != null) {
      this.RegisterCallback(onClick);
    }
  }
}
