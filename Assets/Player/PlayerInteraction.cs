using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance;
    public LayerMask interactionLayerMask;
    public Canvas playerCanvas;
    public RectTransform interactionInfoBackground;
    public Text interactionInfoText;
    public Transform heldTargetTransform;
    public float heldObjectTrackSpeedCoef, heldObjectTrackSpeedMax, heldObjectRotSpeedCoef, heldObjectRotSpeedMax, dropDist;

    private Camera playerCamera;

    private GameObject heldObject;
    const KeyCode PAUSE_KEY = KeyCode.Escape;

    // Start is called before the first frame update
    void Start() {
        playerCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(PAUSE_KEY)) {
            if (Time.timeScale == 0) PauseManager.pauseManagerInstance.UnpauseGame();
            else PauseManager.pauseManagerInstance.PauseGame();
        }

        if (!PauseManager.pauseManagerInstance.IsPaused()) {
            bool grabButtonPressed = Input.GetMouseButtonDown(0);
            RaycastHit hit;
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactionDistance, Color.red);
            interactionInfoText.text = "";
            interactionInfoBackground.gameObject.SetActive(false);
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance, interactionLayerMask)) {
                GameObject obj = hit.collider.gameObject;
                Interactible objInterData = obj.GetComponent<Interactible>();
                if (objInterData) {
                    if (true || objInterData.gameObject != heldObject) {
                        interactionInfoBackground.anchoredPosition = getInteractibleCenterPos(objInterData);
                        interactionInfoText.text = objInterData.interactionText;
                        interactionInfoBackground.gameObject.SetActive(true);
                    }

                    if (grabButtonPressed) {
                        grabButtonPressed = false;
                        if (heldObject) {
                            heldObject.GetComponent<Interactible>().drop();
                            heldObject = null;
                        }
                        else {
                            heldObject = obj;
                            heldObject.GetComponent<Interactible>().grab();
                        }
                    }

                }
            }
            if (grabButtonPressed) {
                if (heldObject) {
                    heldObject.GetComponent<Interactible>().drop();
                    heldObject = null;
                }
            }
        if (heldObject)
        {

            Vector3 distApart = heldTargetTransform.position - heldObject.transform.position + new Vector3(0, 0.7f, 0);
            if (distApart.sqrMagnitude >= Mathf.Pow(dropDist, 2))
            {
                heldObject.GetComponent<Interactible>().drop();
                heldObject = null;
            }
            else
            {
                if (distApart.sqrMagnitude > Mathf.Pow(0.01f, 2))
                {
                    heldObject.GetComponent<Rigidbody>().velocity = distApart * heldObjectTrackSpeedCoef;
                    if (heldObject.GetComponent<Rigidbody>().velocity.sqrMagnitude > Mathf.Pow(heldObjectTrackSpeedMax, 2))
                    {
                        heldObject.GetComponent<Rigidbody>().velocity = distApart.normalized * heldObjectTrackSpeedMax;
                    }
                }
                else
                {
                    heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }

                Vector3 eulerAngles = heldObject.transform.localEulerAngles;
                eulerAngles = new Vector3(
                    eulerAngles.x >= 180 ? eulerAngles.x - 360 : eulerAngles.x,
                    eulerAngles.y >= 180 ? eulerAngles.y - 360 : eulerAngles.y,
                    eulerAngles.z >= 180 ? eulerAngles.z - 360 : eulerAngles.z);
                Vector3 angularDist = -eulerAngles;
                Debug.Log(angularDist + " | " + angularDist.magnitude);
                if (angularDist.sqrMagnitude > Mathf.Pow(5f, 2f))
                {
                    heldObject.GetComponent<Rigidbody>().angularVelocity = angularDist * heldObjectRotSpeedCoef;
                    if (heldObject.GetComponent<Rigidbody>().angularVelocity.sqrMagnitude > Mathf.Pow(heldObjectRotSpeedMax, 2))
                    {
                        heldObject.GetComponent<Rigidbody>().angularVelocity = angularDist.normalized * heldObjectRotSpeedMax;
                    }
                }
                else
                {
                    heldObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                }
            }
        }
        }
    }

    private Vector2 getInteractibleCenterPos(Interactible interactible)
    {
        return playerCamera.WorldToScreenPoint(interactible.getInteractionTextPosition()) / playerCanvas.scaleFactor;
    }
}
