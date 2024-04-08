using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        GameObject saveManagerObj = GameObject.Find("SaveManager");
        if(other.tag == "Player") {
            SaveManager saveManager = saveManagerObj.GetComponent<SaveManager>();
            SaveData currentSave = saveManager.GetCurrentSaveData();
            if (currentSave.GameBeat){
                SceneManager.LoadScene("MainMenu");
            } else {
                int currentSceneName = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene((currentSceneName + 1) % SceneManager.sceneCountInBuildSettings);
            }
            
        }
    }
}
