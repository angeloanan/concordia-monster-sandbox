using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MonsterName : MonoBehaviour
{
    public static string textFieldValue;
    private void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
        // Get the reference to the text field
        TextField textField = root.Q<TextField>("MonsterName");
        
        // Assign a value to the textFieldValue when the text field changes
        textField.RegisterValueChangedCallback((evt) =>
        {
            textFieldValue = evt.newValue;
        });
        
        root.Q<Button>("ContinueButton").clicked += () =>
        {
            Debug.Log("Button Clicked");
            
            // Print the text field value to the console
            Debug.Log(textFieldValue);
            
            SceneManager.LoadScene("Scenes/GameWorld");
        };
        
    }
}