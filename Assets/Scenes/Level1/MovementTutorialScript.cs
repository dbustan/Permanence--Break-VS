using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementTutorialScript : MonoBehaviour
{
    public PlayerControllerPhysics playerController;
    public float startDelay;
    public DialogueBox dialogueBox;
    private float startTime;
    private int stage;


    void Start() {
        playerController.setMovementEnabled(false);
        startTime = Time.time;
        stage = 0;
        Debug.Log(dialogueBox);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime >= startDelay) {
            dialogueBox.showDialogue("Look around using the mouse.");
            playerController.lookThresholdReached += actionComplete;
            startTime = float.MaxValue;
        }
        if(Input.GetKeyDown(KeyCode.D)) {
            actionComplete(null, null);
        }
    }

    private void actionComplete(object sender, EventArgs e) {
        switch(stage) {
            case 0:
                playerController.lookThresholdReached -= actionComplete;
                playerController.moved += actionComplete;
                playerController.setMovementEnabled(true);
                dialogueBox.showDialogue("Move using the right mouse button.");
                break;
            case 1:
                playerController.moved -= actionComplete;
                playerController.changedSpeed += actionComplete;
                dialogueBox.showDialogue("Change your speed using the scroll wheel.");
                break;
            case 2:
                playerController.changedSpeed -= actionComplete;
                playerController.walkedBackwards += actionComplete;
                dialogueBox.showDialogue("Walk backwards by changing your speed.");
                break;
            case 3:
                playerController.walkedBackwards -= actionComplete;
                playerController.jumped += actionComplete;
                dialogueBox.showDialogue("Jump using the middle mouse button.");
                break;
            case 4:
                playerController.jumped -= actionComplete;
                dialogueBox.hideDialogue();
                return;
            default:
                dialogueBox.hideDialogue();
                return;
        }
        stage++;
    }
}
