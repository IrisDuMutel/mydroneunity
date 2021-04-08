using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertialFrame : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform _transform;
    public Transform drone_trans;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   _transform.position = new Vector3(drone_trans.position.x,drone_trans.position.y,drone_trans.position.z);
        _transform.eulerAngles = new Vector3(180f,drone_trans.eulerAngles.y+90f,0.0f);
    }
}
