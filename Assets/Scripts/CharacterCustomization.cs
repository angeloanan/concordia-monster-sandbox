using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomization : MonoBehaviour {
  [SerializeField] private GameObject[][] models;
  
  private modelsActiveIndex = 0;
  
  // Start is called before the first frame update
  void Start() {
    
    foreach (GameObject model in models) {
      model.SetActive(false);
    }
      
    models[activeModelIndex].SetActive(true);
  }

  // Update is called once per frame
  void Update() {
    
  }
}