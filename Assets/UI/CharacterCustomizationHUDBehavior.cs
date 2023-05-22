using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterCustomizationHUDBehavior : MonoBehaviour
{
    private void OnEnable() {
        string[] types = { "Body Type", "Eyes", "Mouth", "Ears", "Horns", "Wings", "Tail", "Fur", "Color" };
    
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        VisualElement customizationWindow = root.Q<VisualElement>("CustomizationWindow");
        VisualElement customizationWindowContent = customizationWindow.Q<VisualElement>("ContentContainer");
        
        foreach (string type in types) {
            var entry = new CharacterCustomizationListEntry(type, _ => { Debug.Log("Clicked!"); } );
            customizationWindowContent.Add(entry);
        }
    }
}
