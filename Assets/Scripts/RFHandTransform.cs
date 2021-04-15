using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RFHandTransform : MonoBehaviour
{

    private Quaternion orient;
    private Vector3 posit;
    private Vector3 lin_vel;
    private Vector3 ang_vel;

    
    public Transform _flMotor;
    public Transform _rlMotor;
    public Transform _frMotor;
    public Transform _rrMotor;

    // public RFHandTransform(Transform transform, Rigidbody rigidB)
    // {
    //    _transf = transform;
    //    _rigidB = rigidB;
    // }
    public void Right2Left(Transform _transform, Rigidbody _rb , Vector3 pos, Quaternion rot, Vector3 vel, Vector3 ang)
    {
        // // Orientation
        _transform.rotation = new Quaternion(-rot[1],-rot[3],-rot[2],rot[0]);

        // Position
        _transform.position = new Vector3(pos[0],pos[2],pos[1]);

        // // Velocity lin
        // var vel = _rb.velocity;
        // _rb.velocity = new Vector3(vel[0],vel[2],vel[1]);

        // // Velocity ang
        // var ang = _rb.angularVelocity;
        // _rb.angularVelocity = new Vector3(-1f*ang[0],-1f*ang[2],-1f*ang[1]);

    }

    public (Quaternion orient, Vector3 posit, Vector3 lin_vel, Vector3 ang_vel) Left2Right(Transform _transf, Rigidbody _rigidB)
    {
        
        
        // Orientation

        var rot = _transf.rotation;
        Debug.Log(rot);
        orient = new Quaternion(-1f*rot.x,-1f*rot.z,-1f*rot.y,rot.w);

        // Position
        var pos = _transf.position;
        posit = new Vector3(pos[0],pos[2],pos[1]);

        // Velocity lin
        var vel = _rigidB.velocity;
        lin_vel = new Vector3(vel[0],vel[2],vel[1]);

        // Velocity ang
        var ang = _rigidB.angularVelocity;
        ang_vel = new Vector3(-1f*ang[0],-1f*ang[2],-1f*ang[1]);

        return (orient,posit,lin_vel,ang_vel);
    }

    public void ForceOnLeft(Transform _transform, Rigidbody _rb , Vector4 forces, float yaw)
    {
        // // Orientation
        // _transform.rotation = new Quaternion(-rot[1],-rot[3],-rot[2],rot[0]);

        // // Position
        // _transform.position = new Vector3(pos[0],pos[2],pos[1]);

        _rb.AddForceAtPosition(_transform.up * forces.x, _flMotor.position, ForceMode.Impulse);
        _rb.AddForceAtPosition(_transform.up * forces.y, _frMotor.position, ForceMode.Impulse);
        _rb.AddForceAtPosition(_transform.up * forces.z, _rlMotor.position, ForceMode.Impulse);
        _rb.AddForceAtPosition(_transform.up * forces.w, _rrMotor.position, ForceMode.Impulse);
        // // Velocity lin
        // var vel = _rb.velocity;
        // _rb.velocity = new Vector3(vel[0],vel[2],vel[1]);

        // // Velocity ang
        // var ang = _rb.angularVelocity;
        // _rb.angularVelocity = new Vector3(-1f*ang[0],-1f*ang[2],-1f*ang[1]);

    }
}
