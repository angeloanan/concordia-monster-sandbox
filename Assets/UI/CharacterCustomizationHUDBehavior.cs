using UnityEngine;
using UnityEngine.UIElements;

public class CharacterCustomizationHUDBehavior : MonoBehaviour
{
    private void OnEnable() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement customizationWindow = root.Q<VisualElement>("CustomizationWindow");
        VisualElement customizationWindowContent = customizationWindow.Q<VisualElement>("ContentContainer");
        
        customizationWindowContent.Add(new CharacterCustomizationListEntry());
    }
}
