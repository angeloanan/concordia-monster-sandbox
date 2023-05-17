using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomization : MonoBehaviour
{
  public GameObject[] body;
  private int currentBody;
  
  public GameObject[] mouth;
  private int currentMouth;
  
  public GameObject[] eyes;
  private int currentEyes;
  
  public GameObject[] nose;
  private int currentNose;
  
  public GameObject[] wing;
  private int currentWing;

  private void Update()
  {
    for (int i = 0; i < body.Length; i++)
    {
      if (i == currentBody)
      {
        body[i].SetActive(true);
      }
      else
      {
        body[i].SetActive(false);
      }
    }
    
    for (int i = 0; i < eyes.Length; i++)
    {
      if (i == currentEyes)
      {
        eyes[i].SetActive(true);
      }
      else
      {
        eyes[i].SetActive(false);
      }
    }
    
    for (int i = 0; i < mouth.Length; i++)
    {
      if (i == currentMouth)
      {
        mouth[i].SetActive(true);
      }
      else
      {
        mouth[i].SetActive(false);
      }
    }
    
    for (int j = 0; j < nose.Length; j++)
    {
      if (j == currentNose)
      {
        nose[j].SetActive(true);
      }
      else
      {
        nose[j].SetActive(false);
      }
    }

    for (int i = 0; i < wing.Length; i++)
    {
      if (i == currentWing)
      {
        wing[i].SetActive(true);
      }
      else
      {
        wing[i].SetActive(false);
      }
    }
  }

  public void SwitchEyes()
  {
    if (currentEyes == eyes.Length - 1)
    {
      currentEyes = 0;
    }
    else
    {
      currentEyes++;
    }
  }
  public void SwitchBody()
  {
    if (currentBody == body.Length - 1)
    {
      currentBody = 0;
    }
    else
    {
      currentBody++;
    }
  }
  public void SwitchMouth()
  {
    if (currentMouth == mouth.Length - 1)
    {
      currentMouth = 0;
    }
    else
    {
      currentMouth++;
    }
  }
  public void SwitchNose()
  {
    if (currentNose == nose.Length - 1)
    {
      currentNose = 0;
    }
    else
    {
      currentNose++;
    }
  }
  public void SwitchWing()
  {
    if (currentWing == wing.Length - 1)
    {
      currentWing = 0;
    }
    else
    {
      currentWing++;
    }
  }
  
}