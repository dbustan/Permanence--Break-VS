using System;
using System.IO;
using Assets.SimpleLocalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Config : MonoBehaviour
{
    [Header("Player Root Components")]
    [SerializeField] private GrayscaleSwapper grayscaleSwapper = null;
    [SerializeField] private LocalizationManagerUI localizationManagerUI = null;
    [SerializeField] private PlayerControllerPhysics playerControllerPhysics = null;

    [SerializeField] private Slider mouseSensitivity,  masterVolumeSlider, musicVolumeSlider, sfxVolumeSlider;

    private SaveConfigSerializable currentConfigData;
    private SoundManager soundManager;

    public static Action<SaveConfigSerializable> ConfigUpdatedEvent { get; internal set; }

    private float TRUNCATE = 100f;
    private void Start()
    {
        currentConfigData = SaveManager.instance.GetCurrentConfigData();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        SetUpSlidersToSavedConfig();

        masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);

        Debug.Log("Start Called at Config");
    }

    private void SetUpSlidersToSavedConfig()
    {
        mouseSensitivity.value = currentConfigData.MouseSensitivity;
        masterVolumeSlider.value = currentConfigData.MasterVolume;
        musicVolumeSlider.value = currentConfigData.MusicVolume;
        sfxVolumeSlider.value = currentConfigData.SFXVolume;
    }

    public void ToggleGreyscale()
    {
        currentConfigData.GrayScaleEnabled = !currentConfigData.GrayScaleEnabled;
        ConfigUpdatedEvent?.Invoke(currentConfigData);
        Debug.Log($"Greyscale enabled {currentConfigData.GrayScaleEnabled}");
    }

    public void ChangeLanguage(string newLanguage)
    {
        currentConfigData.Language = newLanguage;
        ConfigUpdatedEvent?.Invoke(currentConfigData);
        localizationManagerUI.SetLocalization(currentConfigData.Language);
        Debug.Log("New Languge : " + currentConfigData.Language);
    }


    public void ChangeSensitivity(float val)
    {
        currentConfigData.MouseSensitivity = Mathf.Round(val * TRUNCATE) / TRUNCATE;
        mouseSensitivity.value = currentConfigData.MouseSensitivity;
        ConfigUpdatedEvent?.Invoke(currentConfigData);
        Debug.Log("Mouse Sensitivity : " + currentConfigData.MouseSensitivity); 
    }

    public void ChangeMasterVolume(float val)
    {
        currentConfigData.MasterVolume = Mathf.Round(val * TRUNCATE) / TRUNCATE;
        soundManager.ChangeMasterVol(val);
        masterVolumeSlider.value = val;
        ConfigUpdatedEvent?.Invoke(currentConfigData);
        Debug.Log("Master Volume: " + currentConfigData.MasterVolume);
    }

    public void ChangeMusicVolume(float val)
    {
        currentConfigData.MusicVolume = Mathf.Round(val * TRUNCATE) / TRUNCATE;
        soundManager.ChangeMusicVol(val);
        musicVolumeSlider.value = val;
        ConfigUpdatedEvent?.Invoke(currentConfigData);
        Debug.Log("Music Volume: " + currentConfigData.MusicVolume);
    }

    public void ChangeSFXVolume(float val)
    {
        currentConfigData.SFXVolume = Mathf.Round(val * TRUNCATE) / TRUNCATE;
        soundManager.ChangeSFXVol(val);
        sfxVolumeSlider.value = val;
        ConfigUpdatedEvent?.Invoke(currentConfigData);
        Debug.Log("SFXVolume: " + currentConfigData.SFXVolume);
    }

    public void UpdateConfigWithSerializable(SaveConfigSerializable savedConfig)
    {
        if (savedConfig == null) return;
        currentConfigData = savedConfig;
        grayscaleSwapper.setGrayscale(currentConfigData.GrayScaleEnabled);
        localizationManagerUI.SetLocalization(currentConfigData.Language);
        playerControllerPhysics.updateSensitivity(currentConfigData.MouseSensitivity);
        SetUpSlidersToSavedConfig();
    }

}
