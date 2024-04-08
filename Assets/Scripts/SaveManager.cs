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

//Class handles all data that needs to be stored.
public class SaveManager : MonoBehaviour
{
    [SerializeField]

    private GameObject soundManagerObj;

    [SerializeField]

    private GameObject saveMenuUI;





    [SerializeField]
    private GameObject optionsMenuUI;
    private SoundManager sm;

    [SerializeField]
    // private GameObject saveSlot1, saveSlot2, saveSlot3;
    private GameObject saveSlot1;
    private SaveData saveData1, saveData2, saveData3;


    private bool inChinese, grayScale;







    [SerializeField]
    private Slider mainMenuMusicSlider, mainMenuMasterSlider, mainMenuSoundSlider;

    [SerializeField]
    SaveData currentSaveData;

    private string path;

    public static SaveManager instance;


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
        saveMenuUI.SetActive(true);
        inChinese = false;
        GenerateBlankSaves();
        sm = soundManagerObj.GetComponent<SoundManager>();
        DontDestroyOnLoad(gameObject);
        path = Application.persistentDataPath;
        CheckData(path);
        SceneManager.sceneLoaded += OnSceneLoaded;

    }


    private void GenerateBlankSaves()
    {
        saveData1 = SaveData.CreateInstance<SaveData>();
        saveData1.saveDataName = "Save1";
        saveData1.currentLevel = "Level1";
        saveData2 = SaveData.CreateInstance<SaveData>();
        saveData2.saveDataName = "Save2";
        saveData2.currentLevel = "Level1";
        saveData3 = SaveData.CreateInstance<SaveData>();
        saveData3.saveDataName = "Save3";
        saveData3.currentLevel = "Level1";
    }
    //Each Slot will have its saveslot updated here
    private void CheckData(string path)
    {

        // GameObject[] saveSlots = { saveSlot1, saveSlot2, saveSlot3 };
        GameObject[] saveSlots = { saveSlot1 };
        SaveData[] saveDatas = { saveData1, saveData2, saveData3 };
        for (int i = 0; i < 3; i++)
        {
            string saveSlotPath = Path.Combine(path, saveDatas[i].saveDataName + ".json");
            if (!File.Exists(saveSlotPath))
            {
                Debug.Log("No File exists for " + saveSlotPath);
            }
            else
            {
                string json = File.ReadAllText(saveSlotPath);
                SaveDataSerializable saveDataSerializable = JsonUtility.FromJson<SaveDataSerializable>(json);
                saveDataSerializable.SetSaveData(saveDatas[i]);

            }
        }





        // string settingsPath = Path.Combine(path, "settings" + ".json");
        // if (File.Exists(settingsPath)){
        //     string json = File.ReadAllText(settingsPath);
        //     SettingsValues SettingsValues = JsonUtility.FromJson<SettingsValues>(json);
        // }
        saveMenuUI.SetActive(false);



    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name != "MainMenu" && scene.name != "Credits" && !currentSaveData.GameBeat)
        {
            UpdateSaveFile(currentSaveData, scene.name);
            CreateSaveJSON();
        }
        else if (scene.name == "Credits")
        {
            Debug.Log("we did it joe!");
            UpdateSaveFile(currentSaveData, scene.name);
            currentSaveData.currentLevel = "none";
            currentSaveData.currentSlotInfo = "Level Select Mode!";
            currentSaveData.GameBeat = true;
        }
    }

    private void UpdateSaveFile(SaveData currentSave, string sceneName)
    {

        currentSave.currentLevel = sceneName;
        currentSave.currentSlotInfo = sceneName;
        Debug.Log("Saving! " + currentSave.saveDataName);
    }


    public SaveData SetCurrentGameSlot(string SaveSlot)
    {
        SaveData[] saveDatas = { saveData1, saveData2, saveData3 };
        Debug.Log(SaveSlot);
        for (int i = 0; i < 3; i++)
        {
            if (SaveSlot == saveDatas[i].saveDataName)
            {
                currentSaveData = saveDatas[i];
                Debug.Log(currentSaveData.saveDataName + " Locked in!");
                break;
            }
        }

        return currentSaveData;
    }

    private void OnApplicationQuit()
    {
        if (currentSaveData != null)
        {
            CreateSaveJSON();
        }

        //CreateSettingsJSON();
    }
    private void CreateSaveJSON()
    {
        SaveDataSerializable saveDataSerializable = new SaveDataSerializable();
        saveDataSerializable.SetSerializableData(currentSaveData);
        string json = JsonUtility.ToJson(saveDataSerializable);
        string specificFilePath = Path.Combine(path, saveDataSerializable.saveDataName + ".json");
        File.WriteAllText(specificFilePath, json);
        Debug.Log("Creating Json...");
    }

    private void CreateSettingsJSON()
    {
        SettingsValues settingsValues = new SettingsValues
        {
            chinese = inChinese,

        };

        string json = JsonUtility.ToJson(settingsValues);
        string specificFilePath = Path.Combine(path, "settings" + ".json");
        File.WriteAllText(specificFilePath, json);
    }

    public void InChinese()
    {
        inChinese = true;
    }

    public void InEnglish()
    {
        inChinese = false;
    }

    public void ToggleGrayscale()
    {
        grayScale = !grayScale;
    }
    public SaveData GetCurrentSaveData()
    {
        return currentSaveData;
    }


}


[System.Serializable]
public class SettingsValues
{
    // public double masterVol;
    // public double musicVol;
    // public double soundVol;
    // public float masterSliderVal;
    // public float musicSliderVal;
    // public float soundSliderVal;
    // public float originalMusicVol;
    // public float originalSoundVol;

    public bool chinese;


}



