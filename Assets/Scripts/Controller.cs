using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes;
using Twi = RosMessageTypes.Geometry.Twist;

using RosMessageTypes.RoboticsDemo;

// Used by BasicControl.cs
// This script is in charge of getting the commands from user and saving them inside
// its own variables for later use.
public class Controller : MonoBehaviour
{
    // To use ROS command in: roll, pitch, yaw and vertical vel
    private float roll_command;
    private float pitch_command;
    private float yaw_command;
    private float h_vel_command;
    public bool isAgentControl = false; // For when using RL

    public float Throttle = 0.0f;
    public float Yaw = 0.0f;
    public float Roll = 0.0f;
    public float Pitch = 0.0f;
    public BasicControl BasicControl;

    public enum ThrottleMode { None, LockHeight}

    [Header("Throttle command")]
    public string ThrottleCommand = "Throttle";
    public bool InvertThrottle = true;

    [Header("Yaw command")]
    public string YawCommand = "Yaw";
    public bool InvertYaw = false;

    [Header("Pitch command")]
    public string PitchCommand = "Pitch";
    public bool InvertPitch = false;

    [Header("Roll Command")]
    public string RollCommand = "Roll";
    public bool InvertRoll = true;
    
    void Start() 
    {
        if (BasicControl.UseROSCommands)
        {
            ROSConnection.instance.Subscribe<Twi>("QuadCommands", Comm_callback);
        }
    }
    
    void Comm_callback(Twi data)
    {
        // roll_command  = data.roll;
        // pitch_command = data.pitch;
        // yaw_command   = data.yaw;
        // h_vel_command = data.h_vel;
        roll_command  = (float)data.linear.x;
        pitch_command = (float)data.linear.x;
        yaw_command   = (float)data.linear.z;
        h_vel_command = (float)data.linear.x;

    }

    void Update()
    {
        if (BasicControl.UseROSCommands)
        {
            Throttle = h_vel_command;
            Yaw = yaw_command;
            Pitch = pitch_command;
            Roll = roll_command;
            Debug.Log("AAAAAA");
        }
        // if (!isAgentControl)
        else
        {
            Throttle = Input.GetAxisRaw(ThrottleCommand) * (InvertThrottle ? -1 : 1);
            Yaw = Input.GetAxisRaw(YawCommand) * (InvertYaw ? -1 : 1);
            Pitch = Input.GetAxisRaw(PitchCommand) * (InvertPitch ? -1 : 1);
            Roll = Input.GetAxisRaw(RollCommand) * (InvertRoll ? -1 : 1);
        }
    }
    public void InputAction(float throttle, float pitch, float roll, float yaw)
    {

        if (isAgentControl)
        {
            Throttle = throttle;
            Yaw = yaw;
            Pitch = pitch;
            Roll = roll;
        }
    }
}
