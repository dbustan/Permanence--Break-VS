using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudio : MonoBehaviour
{
   private bool currentOpenState, newOpenState;

   [SerializeField] 

   private AudioSource openDoor, closeDoor;

   private Door door;

   private SoundManager sm;

   private GameObject soundManagerObj;

    void Start()
    {
        soundManagerObj = GameObject.Find("SoundManager");
        sm = soundManagerObj.GetComponent<SoundManager>();

        AudioSource[] doorNoises = GetComponents<AudioSource>();

        openDoor = doorNoises[0];
        closeDoor = doorNoises[1];
        door = GetComponent<Door>();
        currentOpenState = door.getOpenState();
    }

    // Update is called once per frame
    void Update()
    {
        newOpenState = door.getOpenState();
        if (currentOpenState != newOpenState){
            currentOpenState = newOpenState;
            if (currentOpenState){
                sm.playAudio(openDoor, "Sound");
            } else {
                sm.playAudio(closeDoor, "Sound");
            }
            
        }
    }
}
