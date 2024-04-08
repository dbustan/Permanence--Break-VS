using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ClosestLight : MonoBehaviour
{

    private List<Transform> lightTransforms;
    public Material cellMat;
    public float transitionTime;
    private Vector3 previousClosestLightPos;
    private float lightChangeStartTime;
    private bool changingLight;
    private Transform closestLight;

    // Start is called before the first frame update
    void Start()
    {
        lightTransforms = new List<Transform>();
        foreach(Light obj in FindObjectsOfType(typeof(Light)) as Light[]) {
            lightTransforms.Add(obj.transform);
            closestLight = obj.transform;
        }
        foreach(Transform lightTransform in lightTransforms) {
            if(lightTransform == closestLight) continue;
            if((transform.position-lightTransform.position).sqrMagnitude < (transform.position-closestLight.position).sqrMagnitude) {
                closestLight = lightTransform;
            }
        }
        changingLight = false;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform lightTransform in lightTransforms) {
            if(closestLight == lightTransform) continue;
            if((transform.position-lightTransform.position).sqrMagnitude < (transform.position-closestLight.position).sqrMagnitude) {
                if(changingLight) {
                    previousClosestLightPos = Vector3.Lerp(previousClosestLightPos, closestLight.position, (Time.time - lightChangeStartTime) / transitionTime);
                } else {
                    previousClosestLightPos = closestLight.position;
                    changingLight = true;
                }
                lightChangeStartTime = Time.time;
                closestLight = lightTransform;
            }
        }

        float currentT = (Time.time - lightChangeStartTime) / transitionTime;
        Vector3 lightPosition = closestLight.position;
        if(changingLight) {
            if(currentT >= 1) {
                changingLight = false;
            } else {
                lightPosition = Vector3.Lerp(previousClosestLightPos, lightPosition, currentT);
            }
        }

        cellMat.SetVector("_CurrentLightPosition", new Vector4(lightPosition.x, lightPosition.y, lightPosition.z, 0f));
    }
}
