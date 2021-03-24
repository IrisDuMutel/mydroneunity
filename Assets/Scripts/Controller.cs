using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used by BasicControl.cs
// This script is in charge of getting the commands from user and saving them inside
// its own variables for later use.
public class Controller : MonoBehaviour
{
    public bool isAgentControl = false; // For when using RL

    public float Throttle = 0.0f;
    public float Yaw = 0.0f;
    public float Roll = 0.0f;
    public float Pitch = 0.0f;

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

    void Update()
    {
        if (!isAgentControl)
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
