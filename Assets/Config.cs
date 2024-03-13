using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Config : MonoBehaviour
{

        [SerializeField] private Slider masterVol, musicVol, soundVol;
        private GameObject soundManagerObj;
        
        private SoundManager sm;
        private void Start() {
            soundManagerObj = GameObject.Find("SoundManager");
            sm = soundManagerObj.GetComponent<SoundManager>();
            masterVol.value = sm.GetMasterVol();
            musicVol.value = sm.GetMusicVol();
            soundVol.value = sm.GetSoundvol();


            masterVol.onValueChanged.AddListener(sm.ChangeMasterVol);
            musicVol.onValueChanged.AddListener(sm.ChangeMusicVol);
            soundVol.onValueChanged.AddListener(sm.ChangeSoundVol);
        }



    
}
