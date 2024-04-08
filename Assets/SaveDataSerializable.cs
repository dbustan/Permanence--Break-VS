using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataSerializable
{
    public string saveDataName;
    public string currentLevel;

    public string currentSlotInfo;

 



    public bool GameBeat;
    

    public void SetSerializableData(SaveData saveData){
        saveDataName = saveData.saveDataName;
        currentLevel = saveData.currentLevel;
        currentSlotInfo = saveData.currentSlotInfo;
        GameBeat = saveData.GameBeat;
        
    }
    public void SetSaveData(SaveData saveData){
        saveData.saveDataName = saveDataName;
        saveData.currentLevel = currentLevel;
        saveData.currentSlotInfo = currentSlotInfo;
        saveData.GameBeat = GameBeat;
    }
    
}
