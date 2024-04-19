using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private Config config;
    private SaveDataSerializable currentSaveData;
    private SaveConfigSerializable currentConfigData;
    private string path;
    public static SaveManager instance;

    public delegate void ConfigUpdatedDelegate(SaveConfigSerializable configData);
    public static event ConfigUpdatedDelegate ConfigUpdatedEvent;


    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        path = Application.persistentDataPath;
        SetUpConfigInScene();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetUpConfigInScene();

        if (scene.name != "MainMenu" && scene.name != "Credits" && !currentSaveData.GameBeat)
        {
            UpdateSaveFile(currentSaveData, scene.name);
            CreateSaveJSON(currentSaveData);
        }
        else if (scene.name == "Credits")
        {
            UpdateSaveFile(currentSaveData, scene.name);
            currentSaveData.currentLevel = "none";
            currentSaveData.currentSlotInfo = "Level Select Mode!";
            currentSaveData.GameBeat = true;
            CreateSaveJSON(currentSaveData);
        }
    }

    private void SetUpConfigInScene()
    {
        if (SceneManager.GetActiveScene().name == "Credits")
            return;

        string configPath = $"{Application.persistentDataPath}/config.json";
        Config config = FindFirstObjectByType<Config>(FindObjectsInactive.Include);
        Debug.Log(configPath);

        if (File.Exists(configPath))
        {
            string json = File.ReadAllText(configPath);

            SaveConfigSerializable saveConfigSerialzable = JsonUtility.FromJson<SaveConfigSerializable>(json);
            config.UpdateConfigWithSerializable(saveConfigSerialzable);
        }
        else
        {
            Debug.Log("File doesn't exist");
            GenerateNewSerializable();
            Debug.Log("This Config: " + this.config);
            config.UpdateConfigWithSerializable(currentConfigData);
            Debug.Log("Found Config:" + config);
            CreateSaveJSON(currentConfigData);
        }
    }

    private void GenerateNewSerializable()
    {
        currentConfigData = new SaveConfigSerializable()
        {
            GrayScaleEnabled = false,
            Language = "English",
            MouseSensitivity = .5f,
            MasterVolume = 1,
            MusicVolume = 1,
            SFXVolume = 1,
        };
    }

    private void UpdateSaveFile(SaveDataSerializable currentSave, string sceneName)
    {
        currentSave.currentLevel = sceneName;
        currentSave.currentSlotInfo = sceneName;
    }

    public void UpdateConfigFile(SaveConfigSerializable currentConfig)
    {
        this.currentConfigData = currentConfig;
    }

    private void CreateSaveJSON(SaveDataSerializable data)
    {
        string json = JsonUtility.ToJson(data);
        string specificFilePath = Path.Combine(path, data.saveDataName + ".json");
        File.WriteAllText(specificFilePath, json);
        Debug.Log("Creating Json...");
    }

    private void CreateSaveJSON(SaveConfigSerializable data)
    {
        string json = JsonUtility.ToJson(data);
        string specificFilePath = Path.Combine(path, "config.json");
        File.WriteAllText(specificFilePath, json);
        Debug.Log("Creating Config Json...");
    }

    public SaveDataSerializable GetCurrentSaveData()
    {
        return currentSaveData;
    }

    public void SetCurrentSaveData(SaveDataSerializable currentSave)
    {
        currentSaveData = currentSave;
    }

    public SaveConfigSerializable GetCurrentConfigData()
    {
        return currentConfigData;
    }

    public void SetCurrentConfigData(SaveConfigSerializable currentConfig)
    {
        currentConfigData = currentConfig;
    }

    private void OnEnable()
    {
        Config.ConfigUpdatedEvent += OnConfigUpdated;
    }

    private void OnDisable()
    {
        Config.ConfigUpdatedEvent -= OnConfigUpdated;
    }

    private void OnConfigUpdated(SaveConfigSerializable configData)
    {
        UpdateConfigFile(currentConfigData);
        CreateSaveJSON(currentConfigData);
    }

}
