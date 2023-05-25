using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button; 
public class UIController : MonoBehaviour
{
    public Button type1Button;
    public TextField type1Text;
    
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        type1Button = root.Q<Button>("type1-button");
        type1Text = root.Q<TextField>("type1-text");
        type1Button.clicked += type1ButtonPressed;
    }

    void type1ButtonPressed()
    {
        if (type1Text.style.display == DisplayStyle.Flex)
        {
            type1Text.style.display = DisplayStyle.None;
        }
        else
        {
            type1Text.style.display = DisplayStyle.Flex;
        }
    }
    
}
