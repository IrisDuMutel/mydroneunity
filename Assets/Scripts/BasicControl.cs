using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using RosMessageTypes.RoboticsDemo;

public class BasicControl : MonoBehaviour
{
    // To use ROS forces command
    private float FLM_force;
    private float FRM_force;
    private float RLM_force;
    private float RRM_force;
    private float yaw_value;

    // To use ROS command in: roll, pitch, yaw and vertical vel
    private float roll_command;
    private float pitch_command;
    private float yaw_command;
    private float h_vel_command;

    [Header("Control")]
    public Controller Controller;
    public float ThrottleIncrease;

    [Header("Motors")]
    public Motor[] Motors;
    public float ThrottleValue;

    [Header("Internal")]
    public ComputerModule Computer;

    [Header("Input")]

    public bool UseROSForces; // True when using ROS to command the drone
    public bool UseROSCommands; // True when commanding yaw,pitch,roll and vertical vel

    void Start() 
    {
        if (UseROSForces)
        {
            ROSConnection.instance.Subscribe<QuadForce>("motorForce", Force_callback);
        }
       

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

// This option uses ROS force commands to move the drone. The forces are applied in the positions
// of the motors. Yaw is handled separately.

        if (UseROSForces)
        {
            // Computer.UpdateComputer(0.0f, 0.0f, 0.1f * ThrottleIncrease);
            // ThrottleValue = Computer.HeightCorrection;
            ComputeMotorsROS();
            if (Computer != null)
                Computer.UpdateGyro();
            ComputeMotorSpeeds();
        }

// This is the option in which roll, pitch, yaw and vertical velocity commands are given through ROS
// Control is included in in this option.

        else if (UseROSCommands)
        {
            Computer.UpdateComputer(Controller.Pitch, Controller.Roll, Controller.Throttle * ThrottleIncrease);
            ThrottleValue = Computer.HeightCorrection;
            ComputeMotors();
            if (Computer != null)
                Computer.UpdateGyro();
            ComputeMotorSpeeds();

        }

// Keyboard control of the drone. No ROS involved.

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
            if (UseROSForces)
            {
                Transform t = motor.GetComponent<Transform>();
                i++;
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
                yaw += YawNormalize(motor,yaw_value, 1.5f);
                
            }

            else
            {
                motor.UpdateForceValues();
                yaw += motor.SideForce;
                i++;
                Transform t = motor.GetComponent<Transform>();
                //			Debug.Log (i);
                // Debug.Log (motor.UpForce);
                rb.AddForceAtPosition(transform.up * motor.UpForce, t.position, ForceMode.Impulse); 
            }
            
        }
        rb.AddTorque(Vector3.up * yaw, ForceMode.Force);
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
            yaw += YawNormalize(motor,yaw_value, 1.5f);
            i++;
            
            //			Debug.Log (i);
            // Debug.Log (motor.UpForce);
            
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