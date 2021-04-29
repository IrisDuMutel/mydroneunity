// Expose tensor of inertia to allow adjustment from
// the inspector.
using UnityEngine;
using System.Collections;

public class InertiaTensor : MonoBehaviour
{
    public Vector3 tensor;
    public Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.inertiaTensor = new Vector3(0.002078f,0.001266f,0.002941f);
        rb.centerOfMass = new Vector3(0.0f,0.0f,0.0f);
    }
}
