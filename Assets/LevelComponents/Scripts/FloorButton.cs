using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    private static int nextButtonId = 0;

    public LayerMask layerMask;
    public GameObject buttonTriggerObj;
    public Renderer particleFieldRenderer;
    public Material unpressedMat, pressedMat;
    private ButtonTrigger buttonTrigger;

    private int unpressedFrames;
    const int maxUnpressedFrames = 4;
    private bool colliding, pressed;
    private int buttonId;

    void Start() {
        buttonId = nextButtonId++;
        if(buttonTriggerObj) {
            buttonTrigger = buttonTriggerObj.GetComponent<ButtonTrigger>();
        }
        unpressedFrames = 0;
        colliding = false;
        pressed = false;
        particleFieldRenderer.sharedMaterial = unpressedMat;
    }

    void FixedUpdate() {
        if(!colliding) {
            unpressedFrames++;
            if(pressed && unpressedFrames >= maxUnpressedFrames) {
                releaseButton();
            }
        } else {
            unpressedFrames = 0;
            if(!pressed) {
                pressButton();
            }
        }
        colliding = false;
    }
    private void OnTriggerStay(Collider other) {
        GameObject obj = other.gameObject;
        if(layerMask != (layerMask | (1 << obj.layer))) return;
        if(obj.GetComponent<VisibilityObject>() && obj.GetComponent<VisibilityObject>().phasedOut) return;
        colliding = true;
    }

    private void pressButton() {
        pressed = true;
        particleFieldRenderer.sharedMaterial = pressedMat;
        if(buttonTrigger != null) {
            buttonTrigger.buttonPress(buttonId);
        }
    }
    private void releaseButton() {
        pressed = false;
        particleFieldRenderer.sharedMaterial = unpressedMat;
        if(buttonTrigger != null) {
            buttonTrigger.buttonRelease(buttonId);
        }
    }
}

public interface ButtonTrigger {
    abstract void buttonPress(int id);
    abstract void buttonRelease(int id);
}
