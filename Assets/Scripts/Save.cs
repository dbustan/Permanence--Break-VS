using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Save : MonoBehaviour
{

   
   [SerializeField] public SaveManager saveManager;

   [SerializeField] private MenuScript menuManager;

   [SerializeField] private GameObject LevelSelectUI;

   [SerializeField] private TextMeshProUGUI saveDescription;

   private SaveData save; 

   private void Awake(){

   }
   private void Start(){
      
   }
   public void OnClick(){
      save = saveManager.SetCurrentGameSlot(gameObject.name);
      if (save.GameBeat){
         menuManager.OpenLevelSelect();
      } else {
         SceneManager.LoadScene(save.currentLevel);
      }
      
       
   }

   private void GenerateBlankSaveSlot(){
      save = SaveData.CreateInstance<SaveData>();
      save.saveDataName = gameObject.name;
      //saveDescription.text = save.currentSlotInfo;
   }

   public SaveData GetSaveData(){
      return save;
   }

   public void SetSaveData(SaveData newSave){
      save = newSave;
      saveDescription.text = save.currentSlotInfo;
   }




   

    
}
