using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCustomization : MonoBehaviour
{
    public Color[] characterColors;
    public Material characterMat;

    public void ChangeColor(int colorIndex)
    {
        characterMat.color = characterColors[colorIndex];
    }
}
