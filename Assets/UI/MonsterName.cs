using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MonsterName : MonoBehaviour
{
    private void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        root.Q<Button>("ContinueButton").clicked += () =>
        {
            Debug.Log("Button Clicked");
            SceneManager.LoadScene("Scenes/GameWorld");
        };
        
    }
}