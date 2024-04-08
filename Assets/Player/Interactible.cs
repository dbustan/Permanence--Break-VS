using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Interactible : MonoBehaviour
{

    public String interactionText;
    public Vector3 interactionTextOffset;
    public PhysicMaterial grabbedPhysicsMat;
    public bool grabbed;
    private PhysicMaterial normalPhysicsMat;

    private Rigidbody rb;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        grabbed = false;
    }

    public void grab()
    {
        if (GetComponent<VisibilityObject>())
        {
            GetComponent<VisibilityObject>().grab();
        }
        rb.mass = 0;
        normalPhysicsMat = GetComponent<Collider>().material;
        GetComponent<Collider>().material = grabbedPhysicsMat;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        grabbed = true;
    }
    public void drop()
    {
        rb.useGravity = true;
        rb.mass = 1;
        grabbed = false;
        GetComponent<Collider>().material = normalPhysicsMat;
        if (GetComponent<VisibilityObject>())
        {
            GetComponent<VisibilityObject>().drop();
        }
    }

    public Vector3 getInteractionTextPosition()
    {
        interactionText = GetComponent<Text>().text;
        return transform.position + interactionTextOffset;
    }
}
