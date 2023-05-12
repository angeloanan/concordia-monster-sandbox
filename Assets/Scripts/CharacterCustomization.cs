using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomization : MonoBehaviour {
  [SerializeField] private GameObject[][] modelGroup;
  [SerializeField] private int[] activeIndexes = {0};
  
  // Start is called before the first frame update
  void Start() {
    // Toggle all models off but set the initial indexes to be on
    foreach (GameObject[] group in modelGroup) {
      foreach (GameObject model in group) {
        model.SetActive(false);
      }
      
      group[0].SetActive(true);
    }
  }

  // Update is called once per frame
  void Update() {
  }

}