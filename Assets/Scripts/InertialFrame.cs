using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertialFrame : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform _transform;
    public Transform drone_body;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
     {  // _transform.position = new Vector3(drone_body.position.x,drone_body.position.y,drone_body.position.z);
        _transform.eulerAngles = new Vector3(0.0f,0.0f,0.0f);
    }
}
