using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SaveConfigSerializable : MonoBehaviour
{
    // Graphics 
    public bool GrayScaleEnabled;

    // Localization 
    public string Language;

    // Player Controls
    public float MouseSensitivity;
    // Audio
    public float MasterVolume;
    public float MusicVolume;
    public float SFXVolume;
}
