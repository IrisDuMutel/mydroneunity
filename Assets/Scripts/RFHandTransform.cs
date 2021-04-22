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

    public (Quaternion orient, Vector3 posit, Vector3 lin_vel, Vector3 ang_vel) Left2Right(Transform _transf, Vector3 inert_linvel, Rigidbody _rigidB)
    {
        
        
        // Orientation

        var rot = _transf.rotation;
        Debug.Log(rot);
        orient = new Quaternion(-1f*rot.x,-1f*rot.z,-1f*rot.y,rot.w);

        // Position
        var pos = _transf.position;
        posit = new Vector3(pos[0],pos[2],pos[1]);

        // Velocity lin
        // var vel = _rigidB.velocity;
        lin_vel = new Vector3(inert_linvel[0],inert_linvel[2],inert_linvel[1]);

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

        _rb.AddForceAtPosition(_flMotor.up * forces.x, _flMotor.position, ForceMode.Force);
        _rb.AddForceAtPosition(_frMotor.up * forces.y, _frMotor.position, ForceMode.Force);
        _rb.AddForceAtPosition(_rlMotor.up * forces.z, _rlMotor.position, ForceMode.Force);
        _rb.AddForceAtPosition(_rrMotor.up * forces.w, _rrMotor.position, ForceMode.Force);
        _rb.AddTorque(_transform.up*-1f*yaw,ForceMode.Force);

        //motor animation:
        // Debug.Log(1000 * Time.deltaTime);
        _flMotor.RotateAround(_flMotor.position, _flMotor.up, -forces.x*100 +1000 * Time.deltaTime);
        _frMotor.RotateAround(_frMotor.position, _frMotor.up,  forces.y*100 +1000 * Time.deltaTime);
        _rlMotor.RotateAround(_rlMotor.position, _rlMotor.up,  forces.z*100 +1000 * Time.deltaTime);
        _rrMotor.RotateAround(_rrMotor.position, _rrMotor.up, -forces.w*100 +1000 * Time.deltaTime);

        // // Velocity lin
        // var vel = _rb.velocity;
        // _rb.velocity = new Vector3(vel[0],vel[2],vel[1]);

        // // Velocity ang
        // var ang = _rb.angularVelocity;
        // _rb.angularVelocity = new Vector3(-1f*ang[0],-1f*ang[2],-1f*ang[1]);

    }
}
