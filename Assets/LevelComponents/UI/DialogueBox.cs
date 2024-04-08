using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{

    public float yTargetIn, yTargetOut, slideTime;
    private float startTime;
    private bool shouldShow, slide;

    private Text text;
    void Start() {
        text = GetComponentInChildren<Text>();
        gameObject.SetActive(false);
        shouldShow = false;
        slide = false;
    }

    void Update() {
        float currentTime = Time.time;
        float timePassed = currentTime - startTime;
        if(!slide || timePassed > slideTime) {
            if(shouldShow) {
                ((RectTransform)transform).anchoredPosition3D = Vector3.up * yTargetIn;
            } else {
                ((RectTransform)transform).anchoredPosition3D = Vector3.up * yTargetOut;
                gameObject.SetActive(false);
            }
            slide = false;
        } else if(slide) {
            if(shouldShow) {
                ((RectTransform)transform).anchoredPosition3D = Vector3.Lerp(Vector3.up * yTargetOut, Vector3.up * yTargetIn, timePassed / slideTime);
            } else {
                ((RectTransform)transform).anchoredPosition3D = Vector3.Lerp(Vector3.up * yTargetIn, Vector3.up * yTargetOut, timePassed / slideTime);
            }
        }
    }

    public void showDialogue(string dialogueString) {
        text.text = dialogueString;
        if(!gameObject.activeSelf) {
            gameObject.SetActive(true);
            shouldShow = true;
            slide = true;
            startTime = Time.time;
        }
    }
    public void hideDialogue() {
        shouldShow = false;
        slide = true;
        startTime = Time.time;
    }
}
