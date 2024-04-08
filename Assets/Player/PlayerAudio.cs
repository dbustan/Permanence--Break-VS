using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource[] footsteps;
    private SoundManager sm;
    private PlayerControllerPhysics playerPhysics;

    private Rigidbody playerRb;
    private GameObject soundManagerGameobj;

    private GameObject footstepsGameObj;

    private float footstepsVol;

    private bool grounded;

    [SerializeField]
    private float minSpeed, maxSpeed;

    [SerializeField]
    private float minInterval, maxInterval;
    private bool timePassed;
    private bool notYetMoved;

    private float timeMoving;

    [SerializeField]
    void Start()
    {
        playerPhysics = GetComponent<PlayerControllerPhysics>();
        playerRb = GetComponent<Rigidbody>();
        footstepsVol = 0.2f;
        timePassed = false;
        timeMoving = 0;
        footstepsGameObj = gameObject.transform.GetChild(2).gameObject;
        footsteps = footstepsGameObj.GetComponents<AudioSource>();
        soundManagerGameobj = GameObject.Find("SoundManager");
        sm = soundManagerGameobj.GetComponent<SoundManager>();
    }


    void Update()
    {
        grounded = playerPhysics.getGrounded();
        if (grounded && playerRb.velocity.magnitude > 0.01)
        {
            timePassed = FootstepsTimer();
            timeMoving += Time.deltaTime;
        }
        if (timePassed || playerRb.velocity.magnitude < 0.001)
        {

            timeMoving = 0;
            timePassed = false;
        }
    }

    private bool FootstepsTimer()
    {

        float speed = playerRb.velocity.magnitude;
        float normalizedSpeed = NormalizeSpeed(speed, minSpeed, maxSpeed);
        float timeBetweenSound = ScaleInterval(normalizedSpeed, minInterval, maxInterval);
        if (timeMoving >= timeBetweenSound)
        {
            sm.playAudio(footsteps, footstepsVol);
            return true;
        }
        else
        {
            return false;
        }



    }
    private float NormalizeSpeed(float speed, float minSpeed, float maxSpeed)
    {
        float normalizedSpeed = (speed - minSpeed) / (maxSpeed - minSpeed);
        normalizedSpeed = Mathf.Clamp(normalizedSpeed, 0, 1);
        return normalizedSpeed;
    }

    private float ScaleInterval(float normalizedSpeed, float minInterval, float maxInterval)
    {
        float range = maxInterval - minInterval;
        float inverseSpeed = 1 - normalizedSpeed;

        float scaledInterval = inverseSpeed * range;
        scaledInterval += minInterval;
        //Debug.Log(scaledInterval);
        return scaledInterval;
    }
}
