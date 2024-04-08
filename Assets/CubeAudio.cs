using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeAudio : MonoBehaviour
{
    private AudioSource[] blockSounds;

    private bool exists;

    private GameObject soundManagerObj;

    private float valueToScale;
    private SoundManager sm;
    private VisibilityObject visibleOfObj;
    void Start()
    {
        visibleOfObj = GetComponent<VisibilityObject>();
        
        blockSounds = GetComponents<AudioSource>();
        soundManagerObj = GameObject.Find("SoundManager");
        sm = soundManagerObj.GetComponent<SoundManager>();
        valueToScale = blockSounds[0].volume;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.name != "Player"){
            if (exists){
                sm.playAudio(blockSounds, valueToScale);
            }
            if (visibleOfObj == null){
                sm.playAudio(blockSounds, valueToScale);
            }
            
        }
    }

    private void OnCollisionExit(Collision other) {
        if (visibleOfObj != null){
            if (visibleOfObj.phasedOut){
                exists = false;
            } else {
                exists = true;
            }
        }
    }

}
