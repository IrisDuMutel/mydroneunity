using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public GameObject FrontLeftMotor;
    public GameObject FrontRightMotor;
    public GameObject RearLeftMotor;
    public GameObject RearRightMotor;
    public Rigidbody rb;

    public float throttleVal = 0f;
    public float sensitivity = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        throttleVal = CalculateThrottle(throttleVal);

        ApplyThrust(throttleVal, throttleVal, throttleVal, throttleVal);


        

    }

    public float CalculateThrottle(float tv)
    {
        tv += Input.GetAxis("Vertical");
        tv = Mathf.Clamp(tv, 0f, 5f);
        Debug.Log(tv);
        return tv = 3f/4f;

    }

    void ApplyThrust(float fl, float fr, float rl, float rr)
    {
        rb.AddForceAtPosition(transform.up * fl, FrontLeftMotor.transform.position);
        rb.AddForceAtPosition(transform.up * rr, RearLeftMotor.transform.position);

        rb.AddForceAtPosition(transform.up * rl, RearLeftMotor.transform.position);
        rb.AddForceAtPosition(transform.up * fr, FrontRightMotor.transform.position);
    }
}
