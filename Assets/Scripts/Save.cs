using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


//Class handles gathering saves
public class Save : MonoBehaviour
{

   
   [SerializeField] public SaveManager saveManager;

   [SerializeField] private TextMeshProUGUI saveDescription;

   private SaveDataSerializable saveSlotData;

   private string currentPath;
   private void Start(){
      saveSlotData = CheckSave();
      if (saveSlotData == null){
         GenerateBlankSaveSlot();
      }
      //Find a save data, else Generate
   }
   public void OnClick(){
      saveManager.SetCurrentSaveData(saveSlotData);
   }

   private SaveDataSerializable CheckSave(){
      currentPath = Application.persistentDataPath + this.gameObject.name + ".json";
      if (File.Exists(currentPath)){
         string json = File.ReadAllText(currentPath);
         SaveDataSerializable currentSaveData = JsonUtility.FromJson<SaveDataSerializable>(json);
         return currentSaveData;
      } else{
         return null;
      }
   }

   private void GenerateBlankSaveSlot(){
      saveSlotData = new SaveDataSerializable();
      saveSlotData.saveDataName = gameObject.name;
      Debug.Log("Generated Save File");
   }






   

    
}
