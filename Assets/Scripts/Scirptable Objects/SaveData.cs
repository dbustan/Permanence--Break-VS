using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  [CreateAssetMenu(fileName = "SaveData", menuName = "ScriptableObjects/SaveData", order = 1)]
  [System.Serializable]
public class SaveData : ScriptableObject
{
    public string saveDataName;
    public string currentLevel;

    public string currentSlotInfo;

    public bool GameBeat;


    
    
}
