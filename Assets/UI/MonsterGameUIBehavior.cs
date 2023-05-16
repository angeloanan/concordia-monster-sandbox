using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterGameUIBehavior : MonoBehaviour
{
    private void OnEnable() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        VisualElement cubeItem = root.Q<VisualElement>("Item");
        
        cubeItem.RegisterCallback<MouseEnterEvent>(e => {
            cubeItem.style.backgroundColor = new StyleColor(new Color(0.8f, 0.8f, 0.8f));
        });
        cubeItem.RegisterCallback<MouseLeaveEvent>(e => {
            cubeItem.style.backgroundColor = new StyleColor(Color.white);
        });
    }
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
