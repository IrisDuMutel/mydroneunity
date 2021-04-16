using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform _transform;
    public Transform drone_trans;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   _transform.position = new Vector3(drone_trans.position.x - 2f,drone_trans.position.y + 0.197f,drone_trans.position.z);
        _transform.eulerAngles = new Vector3(0.0f,90f,0.0f);
    }
}
