using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Odometry = RosMessageTypes.Nav.Odometry;
using RosMessageTypes.RoboticsDemo;

public class ForceSub : MonoBehaviour
{
    public Transform _transform;
    // private Vector3 pos;
    // private Quaternion rot;
    // private Vector3 vel;
    // private Vector3 ang;
    private Vector4 forces;
    private float yaw;
    public Rigidbody _rb;
    public RFHandTransform _rftrans;
    // Start is called before the first frame update
    void Start() 
    {

        ROSConnection.instance.Subscribe<QuadForce>("motorForce", _callback);

    }
    void _callback(QuadForce data)
    {
        // float pos_x = (float)data.pose.pose.position.x;
        // float pos_y = (float)data.pose.pose.position.y;
        // float pos_z = (float)data.pose.pose.position.z;
        
        // float rot_w = (float)data.pose.pose.orientation.w;
        // float rot_x = (float)data.pose.pose.orientation.x;
        // float rot_y = (float)data.pose.pose.orientation.y;
        // float rot_z = (float)data.pose.pose.orientation.z;

        // float vel_x = (float)data.twist.twist.linear.x;
        // float vel_y = (float)data.twist.twist.linear.y;
        // float vel_z = (float)data.twist.twist.linear.z;

        // float ang_x = (float)data.twist.twist.angular.x;
        // float ang_y = (float)data.twist.twist.angular.y;
        // float ang_z = (float)data.twist.twist.angular.z;
        float _flm = data.flm;
        float _frm = data.frm;
        float _rlm = data.rlm;
        float _rrm = data.rrm;
        float _yaw = data.yaw;

        forces = new Vector4(_flm,_frm,_rlm,_rrm);
        yaw = _yaw;
        // pos = new Vector3(pos_x,pos_y,pos_z);
        // rot = new Quaternion(rot_w,rot_x,rot_y,rot_z);
        // vel = new Vector3(vel_x,vel_y,vel_z); 
        // ang = new Vector3(ang_x,ang_y,ang_z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        // _rftrans.Right2Left(_transform,_rb,pos,rot,vel, ang);
        _rftrans.ForceOnLeft(_transform,_rb,forces,yaw);

        
    }
}
