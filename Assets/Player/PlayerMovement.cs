using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float cameraSensitivity;
    public float verticalRange;

    public float speedRange;
    public float gravity, terminalVelocity;
    private float verticalVelocity;
    public float currentSpeed;
    public float speedChangeRate;
    private float currentSpeedPreCurve;
    public float jumpHeight;
    public Image speedReadout;

    private SoundManager soundManager;
    private Camera playerCamera;
    private bool jumping;

    public float moveThreshold = 1.45f;
    private AudioSource [] footsteps;
    private Vector3 lastPos;
    private CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        GameObject soundManagerGameobj = GameObject.Find("SoundManager");
        soundManager = soundManagerGameobj.GetComponent<SoundManager>(); 
        lastPos = transform.position;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerCamera = GetComponentInChildren<Camera>();
        cc = GetComponent<CharacterController>();
        footsteps = GetComponents<AudioSource>();
        speedReadout.rectTransform.localScale = new Vector3(1f, getSpeedReadoutScale(), 1f);
        currentSpeedPreCurve = Mathf.Sqrt(currentSpeed/speedRange);
    }

    
    // Update is called once per frame
    void Update()
    {
        updateSpeed();
        cameraRotation();
        move();
        if(!cc.isGrounded) {
            verticalVelocity = Mathf.Clamp(verticalVelocity - gravity * Time.deltaTime, -terminalVelocity, 1000);
        } else {
            if(!jumping) {
                if(Input.GetMouseButtonDown(2)) {
                    verticalVelocity = Mathf.Sqrt(2*gravity*jumpHeight);
                    jumping = true;
                }
            } else {
                verticalVelocity = 0f;
            }
            if(jumping && verticalVelocity <= 0) {
                jumping = false;
            }
        }
    }

    private void move() {
        
        float forward = Input.GetMouseButton(1) ? 1f : 0f;
        
        Vector3 movement = transform.rotation * new Vector3(0f, 0f, forward) * currentSpeed * Time.deltaTime;
        
        movement += Vector3.up * verticalVelocity * Time.deltaTime;
        float moveDistance = Vector3.Distance(transform.position, lastPos);
        if (moveDistance >= moveThreshold){
            lastPos = transform.position;
      
            Debug.Log("hwdaad");
        }
        
        cc.Move(movement);
    }

  
    private void cameraRotation() {
        Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 rotationDelta = new Vector3(-mouseMovement.y, mouseMovement.x, 0f) * cameraSensitivity * Time.deltaTime;

        float currentPlayerRotationY = transform.localRotation.eulerAngles.y;
        currentPlayerRotationY += rotationDelta.y;
        transform.localRotation = Quaternion.Euler(Vector3.up * currentPlayerRotationY);

        float currentCameraRotationX = playerCamera.transform.localRotation.eulerAngles.x;
        if(currentCameraRotationX >= 180) currentCameraRotationX -= 360;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX + rotationDelta.x, -verticalRange/2, verticalRange/2);
        playerCamera.transform.localRotation = Quaternion.Euler(Vector3.right * currentCameraRotationX);
    }

    private void updateSpeed() {
        currentSpeedPreCurve = Mathf.Clamp(currentSpeedPreCurve + Input.mouseScrollDelta.y * speedChangeRate, -1, 1);
        currentSpeed = speedRange * Mathf.Pow(currentSpeedPreCurve, 2) * Math.Sign(currentSpeedPreCurve);
        speedReadout.rectTransform.localScale = new Vector3(1f, getSpeedReadoutScale(), 1f);
    }
    private float getSpeedReadoutScale() {
        return 6*(currentSpeed/speedRange);
    }
}
