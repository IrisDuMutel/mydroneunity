using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using RosMessageTypes.RoboticsDemo;

public class BasicControl : MonoBehaviour
{
    private float FLM_force;
    private float FRM_force;
    private float RLM_force;
    private float RRM_force;
    private float yaw_value;

    [Header("Control")]
    public Controller Controller;
    public float ThrottleIncrease;

    [Header("Motors")]
    public Motor[] Motors;
    public float ThrottleValue;

    [Header("Internal")]
    public ComputerModule Computer;

    public bool UseROS; // True when using ROS to command the drone

    void Start() 
    {
        if (UseROS)
            ROSConnection.instance.Subscribe<QuadForce>("motorForce", Force_callback);
    }
    
    void Force_callback(QuadForce data)
    {
       FLM_force = data.flm;
       FRM_force = data.frm;
       RLM_force = data.rlm;
       RRM_force = data.rrm;
       yaw_value = data.yaw;
       
    }

// Frame-rate independent MonoBehaviour.FixedUpdate message for physics calculations.
// MonoBehaviour.FixedUpdate has the frequency of the physics system; it is called every fixed frame-rate frame.
// Compute Physics system calculations after FixedUpdate. 0.02 seconds (50 calls per second) is the default time 
// between calls.
    void FixedUpdate()
    {   
        if (UseROS)
        {
            ComputeMotorsROS();
            if (Computer != null)
                Computer.UpdateGyro();
            ComputeMotorSpeeds();
        }

        else
        {
            Computer.UpdateComputer(Controller.Pitch, Controller.Roll, Controller.Throttle * ThrottleIncrease);
            ThrottleValue = Computer.HeightCorrection;
            //		Debug.Log (Computer.PitchCorrection);
            ComputeMotors();
            if (Computer != null)
                Computer.UpdateGyro();
            ComputeMotorSpeeds();
        }

        
    }
    
    private void ComputeMotorsROS()
    {
        float yaw = 0.0f;
        Rigidbody rb = GetComponent<Rigidbody>();
        int i = 0;
        foreach (Motor motor in Motors)
        {
            Transform t = motor.GetComponent<Transform>();

            if (motor.name.Contains("F"))
            {
                if (motor.name.Contains("FL"))
                {
                    rb.AddForceAtPosition(transform.up * FLM_force, t.position, ForceMode.Impulse);
                }
                if (motor.name.Contains("FR"))
                {
                    rb.AddForceAtPosition(transform.up * FRM_force, t.position, ForceMode.Impulse);                    
                }
            if (motor.name.Contains("RL"))
            {
                rb.AddForceAtPosition(transform.up * RLM_force, t.position, ForceMode.Impulse);
            }
            if (motor.name.Contains("RR"))
            {
                rb.AddForceAtPosition(transform.up * RRM_force, t.position, ForceMode.Impulse);
            }
            }
            // motor.UpdateForceValues();
            yaw += YawNormalize(motor,yaw_value, 0.0f);
            i++;
            
            //			Debug.Log (i);
            // Debug.Log (motor.UpForce);
            
        }
        rb.AddTorque(Vector3.up * yaw, ForceMode.Force);
    }

private float YawNormalize(Motor motor,float input, float factor)
	{
		float finalValue = input;

		if (motor.InvertDirection)
			finalValue = Mathf.Clamp(finalValue, -1, 0);
		else
			finalValue = Mathf.Clamp(finalValue, 0, 1);

		return finalValue * (factor);
	}

    private void ComputeMotors()
    {
        float yaw = 0.0f;
        Rigidbody rb = GetComponent<Rigidbody>();
        int i = 0;
        foreach (Motor motor in Motors)
        {
            motor.UpdateForceValues();
            yaw += motor.SideForce;
            i++;
            Transform t = motor.GetComponent<Transform>();
            //			Debug.Log (i);
            // Debug.Log (motor.UpForce);
            rb.AddForceAtPosition(transform.up * motor.UpForce, t.position, ForceMode.Impulse);
        }
        rb.AddTorque(Vector3.up * yaw, ForceMode.Force);
    }

    private void ComputeMotorSpeeds()
    {
        foreach (Motor motor in Motors)
        {
            if (Computer.Gyro.Altitude < 0.1)
                motor.UpdatePropeller(0.0f);
            else
                motor.UpdatePropeller(1200.0f);
        }
    }


    public void Reset()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        Computer.Reset();
        foreach (Motor motor in Motors)
        {
            motor.Reset();
        }
    }
}