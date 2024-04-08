using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* Description (:
    A one-handed Physics based physics based character controller.
    Only uses the mouse, with all instances of keyboard input existing
    solely for playtesting sessions and debuggig.

    Created for the course "CSE 171: Game Design Studio I" at UCSC.
*/

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerControllerPhysics : MonoBehaviour {

    private Rigidbody rb;

    // CAMERA CONTROL
    [Header("Camera Control")]

    public float cameraSensitivity;
    public float verticalRange;
    private Camera playerCamera;
    public Slider sensitivitySlider;


    
    // MOVEMENT
    [Header("Movment")]
    public float maxWalkingSpeed;
    public float walkingAcceleration, walkingDeceleration, walkingSpeedChangeRate, inAirDecelCoef;
    private float currentWalkingSpeed, currentWalkingSpeedPreCurve;

    // MOVEMENT
    [Header("Walking Speed Bar")]
    public RectTransform speedReadout;
    public Color minColor, maxColor;
    private float speedBarScaleHeight = 1f;
    
    // JUMPING
    [Header("Jumping")]
    public float jumpHeight;
    public float gravity, groundCheckDistance, maxStepHeight, stepClimbSpeed;
    public LayerMask groundCheckLayerMask;
    [SerializeField]
    private bool grounded, jumping, wasStep;
    private Vector3[] groundCheckDirections = {
        Vector3.down,
        new Vector3(1, -1, 0).normalized,
        new Vector3(0, -1, 1).normalized,
        new Vector3(-1, -1, 0).normalized,
        new Vector3(0, -1, -1).normalized,
    };

    
    // USER INPUT
    private bool moveFlag, jumpFlag;

    private Vector3 startingPosition;

    // MOVEMENT TUTORIAL
    public event EventHandler lookThresholdReached, moved, changedSpeed, walkedBackwards, jumped;
    private Vector3 startingRotation;
    private bool movementEnabled = true;

    void Start() {
        rb = GetComponent<Rigidbody>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerCamera = GetComponentInChildren<Camera>();

        currentWalkingSpeedPreCurve = 0.75f;

        moveFlag = jumpFlag = false;
        startingPosition = transform.position;
        startingRotation = transform.rotation.eulerAngles;

        cameraSensitivity = SoundManager.instance.getSens();
    }

    void Update() {
        moveFlag = movementEnabled && Input.GetMouseButton(1);
        if(Input.GetMouseButtonDown(2)) {
            jumpFlag = true;
        }
        if(Input.GetKeyDown(KeyCode.Space)) {
            transform.position = startingPosition;
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
        if(Input.GetKeyDown(KeyCode.Q)) {
            Application.Quit();
        }

        /* TUTORIAL EVENT */
        if(lookThresholdReached != null) {
            float deltaY = (transform.rotation.eulerAngles-startingRotation).y;
            if(deltaY > 45 && deltaY < 315) {
                lookThresholdReached.Invoke(this, EventArgs.Empty);
            }
        }
        /* TUTORIAL EVENT */

        updateCamera();
        updateSpeed();
    }

    void FixedUpdate() {
        grounded = isGrounded();
        Vector3 currentVelocity = rb.velocity;

        addWalkingMovement(ref currentVelocity);
        addVerticalMovement(ref currentVelocity);

        rb.velocity = currentVelocity;
    }

    private void addWalkingMovement(ref Vector3 currentVelocity) {
        Vector3 currentVelocityXZ = new Vector3(currentVelocity.x, 0f, currentVelocity.z);
        bool decelerateOverride = false;
        if(moveFlag) {
            float lastMagnitude = currentVelocityXZ.magnitude;
            float absWalkSpeed = Mathf.Abs(currentWalkingSpeed);
            currentVelocityXZ += transform.rotation * Vector3.forward * walkingAcceleration * Mathf.Sign(currentWalkingSpeed) * Time.fixedDeltaTime;
            /* TUTORIAL EVENT */
            if(moved != null) {
                if(currentVelocityXZ.sqrMagnitude > 0) {
                    moved.Invoke(this, EventArgs.Empty);
                }
            }
            /* TUTORIAL EVENT */
            /* TUTORIAL EVENT */
            if(walkedBackwards != null) {
                if(currentWalkingSpeed < 0 && currentVelocityXZ.sqrMagnitude > 0) {
                    walkedBackwards.Invoke(this, EventArgs.Empty);
                }
            }
            /* TUTORIAL EVENT */
            if(lastMagnitude < absWalkSpeed) {
                if(currentVelocityXZ.magnitude > absWalkSpeed) {
                    currentVelocityXZ = currentVelocityXZ.normalized * absWalkSpeed;
                }
            } else {
                decelerateOverride = true;
                currentVelocityXZ = currentVelocityXZ.normalized * lastMagnitude;
            }
        }
        if((!moveFlag || decelerateOverride) && currentVelocityXZ.magnitude > 0f) {
            Vector3 decelDir = -currentVelocityXZ.normalized;
            currentVelocityXZ += decelDir * walkingDeceleration * (grounded ? 1f : inAirDecelCoef) * Time.fixedDeltaTime;
            if(currentVelocityXZ.sqrMagnitude <= 0.01f || Vector3.Dot(currentVelocityXZ, decelDir) >= 0) {
                currentVelocityXZ = Vector3.zero;
            }
        }

        currentVelocity = new Vector3(currentVelocityXZ.x, currentVelocity.y, currentVelocityXZ.z);
    }
    private void updateSpeed() {
        /* TUTORIAL EVENT */
        if(changedSpeed != null) {
            if(Input.mouseScrollDelta.y != 0) {
                changedSpeed.Invoke(this, EventArgs.Empty);
            }
        }
        /* TUTORIAL EVENT */
        currentWalkingSpeedPreCurve = Mathf.Clamp(currentWalkingSpeedPreCurve + Input.mouseScrollDelta.y * walkingSpeedChangeRate, -1, 1);
        currentWalkingSpeed = maxWalkingSpeed * Mathf.Pow(currentWalkingSpeedPreCurve, 2) * Mathf.Sign(currentWalkingSpeedPreCurve);
        speedReadout.localScale = new Vector3(1f, getSpeedReadoutScale(), 1f);
        speedReadout.GetComponent<Image>().color = Color.Lerp(minColor, maxColor, (currentWalkingSpeed / maxWalkingSpeed + 1) / 2f);
    }
    private float getSpeedReadoutScale() {
        return speedBarScaleHeight*(currentWalkingSpeed/maxWalkingSpeed);
    }
    
    private bool isGrounded() {
        bool validHit = false;
        for(int i = 0; i < groundCheckDirections.Length; i++) {
            Vector3 direction = groundCheckDirections[i];
            Vector3 origin = transform.position + Vector3.up * 0.5f + direction * 0.4f;
            Debug.DrawRay(origin, direction * (groundCheckDistance + 0.1f), Color.red);
            RaycastHit hit;
            if(Physics.Raycast(origin, direction, out hit, groundCheckDistance + 0.1f, groundCheckLayerMask)) {
                Interactible interactible = hit.collider.GetComponent<Interactible>();
                if(interactible && interactible.grabbed) continue;
                validHit = true;
            }
        }
        return validHit;
    }
    private bool checkForStep() {
        RaycastHit hit;
        Debug.DrawRay(transform.position + Vector3.up * 0.01f, transform.rotation * Vector3.forward * 0.6f, Color.red);
        Debug.DrawRay(transform.position + Vector3.up * maxStepHeight, transform.rotation * Vector3.forward * 0.6f, Color.red);
        if(Physics.Raycast(transform.position + Vector3.up * 0.01f, transform.rotation * Vector3.forward, out hit, 0.6f, groundCheckLayerMask)) {
            Interactible interactible = hit.collider.GetComponent<Interactible>();
            if(interactible && interactible.grabbed) return false;
            if(Physics.Raycast(transform.position + Vector3.up * maxStepHeight, transform.rotation * Vector3.forward, 0.6f, groundCheckLayerMask)) {
                return false;
            }
            return true;
        }
        return false;
    }
    private void addVerticalMovement(ref Vector3 currentVelocity) {
        if(jumpFlag) {
            tryJump(ref currentVelocity);
        }
        if(!grounded) {
            if(jumping && currentVelocity.y <= 0) {
                jumping = false;
            }
            currentVelocity += Vector3.down * gravity * Time.fixedDeltaTime;
        } else if(!jumping) {
            currentVelocity.y = 0;
        }
        if(checkForStep() && moveFlag) {
            wasStep = true;
            Debug.Log("STEP");
            currentVelocity.y = stepClimbSpeed;
        } else if(wasStep) {
            wasStep = false;
            currentVelocity.y = currentVelocity.y / 4;
        }
    }
    private void tryJump(ref Vector3 currentVelocity) {
        if(grounded && !jumping) {
            /* TUTORIAL EVENT */
            if(jumped != null) {
                jumped.Invoke(this, EventArgs.Empty);
            }
            /* TUTORIAL EVENT */
            currentVelocity += Vector3.up * Mathf.Sqrt(2 * gravity * jumpHeight);
            jumping = true;
        }
        jumpFlag = false;
    }
    
    private void updateCamera() {
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

    public void updateSensitivity(float newSens) {
        cameraSensitivity = newSens;
        SoundManager.instance.updateSens(newSens);
    }

    public void updateSensitivitySlider(float newSens) {
        sensitivitySlider.value = newSens;
    }

    public void setMovementEnabled(bool enabled) {
        movementEnabled = enabled;
    }
    public float getCurrentWalkingSpeed() {
        return currentWalkingSpeed;
    }

    public bool getGrounded(){
        return grounded;
    }

}
