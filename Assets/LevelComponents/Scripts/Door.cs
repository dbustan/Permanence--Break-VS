using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour, ButtonTrigger
{
    public bool defaultOpen;
    public int numButtons = 1;

    private List<int> activeButtons;
    private bool openState;
    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
        activeButtons = new List<int>();
        
        snapDoorOpenState(defaultOpen);
        updateDoor();
    }

    public void buttonPress(int id) {
        if(!activeButtons.Contains(id)) {
            activeButtons.Add(id);
        }
        updateDoor();
    }

    public void buttonRelease(int id) {
        if(activeButtons.Contains(id)) {
            activeButtons.Remove(id);
        }
        updateDoor();
    }

    private void updateDoor() {
        bool newOpenState = shouldOpen();
        if(openState != newOpenState) {
            openState = newOpenState;
            setAnimationSpeed();
        }
    }
    public bool shouldOpen() {
        return (activeButtons.Count >= numButtons) ^ defaultOpen;
    }
    private void setAnimationSpeed() {
        animator.SetFloat("speed", openState ? 1f : -1f);
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 || animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0 && !animator.IsInTransition(0)) {
            animator.SetTrigger("restartAnim");
        }
    }

    private void snapDoorOpenState(bool open) {
        openState = open;
        animator.SetFloat("speed", openState ? 1f : -1f);
        if(openState) {
            animator.CrossFade("DoorArmature|Open", 0f, 0, 1f);
        } else {
            animator.CrossFade("DoorArmature|Open", 0f, 0, 0f);
        }
    }

    public bool getOpenState(){
        return openState;
    }
}
