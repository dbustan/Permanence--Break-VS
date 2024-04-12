using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.SimpleLocalization.Scripts;
using Unity.Collections;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

//Class handles Saves/Creating JSONs for current Save
public class SaveManager : MonoBehaviour
{

    private SaveDataSerializable currentSaveData;

    public static SaveManager instance;

    private string path;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        path = Application.persistentDataPath;
    }



    
    //Each Slot will have its saveslot updated here


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name != "MainMenu" && scene.name != "Credits" && !currentSaveData.GameBeat)
        {
            UpdateSaveFile(currentSaveData, scene.name);
            CreateSaveJSON();
        }
        else if (scene.name == "Credits")
        {
            UpdateSaveFile(currentSaveData, scene.name);
            currentSaveData.currentLevel = "none";
            currentSaveData.currentSlotInfo = "Level Select Mode!";
            currentSaveData.GameBeat = true;
        }
    }

    private void UpdateSaveFile(SaveDataSerializable currentSave, string sceneName)
    {
        currentSave.currentLevel = sceneName;
        currentSave.currentSlotInfo = sceneName;
    }



 
    private void CreateSaveJSON()
    {
        string json = JsonUtility.ToJson(currentSaveData);
        string specificFilePath = Path.Combine(path, currentSaveData.saveDataName + ".json");
        File.WriteAllText(specificFilePath, json);
        Debug.Log("Creating Json...");
    }

  
 
    public SaveDataSerializable GetCurrentSaveData()
    {
        return currentSaveData;
    }

    public void SetCurrentSaveData(SaveDataSerializable currentSave){
        currentSaveData = currentSave;

    }
    


}




