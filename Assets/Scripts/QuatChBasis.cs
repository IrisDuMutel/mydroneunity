using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuatChBasis : MonoBehaviour
{
    public Transform _transf; 
    // Start is called before the first frame update
    private Quaternion rot;
    void Start()
    {
        var rot = _transf.rotation;
        _transf.transform.rotation = new Quaternion(-1f*rot.x,-1f*rot.z,-1f*rot.y,rot.w);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // _transf.Rotate(0,20f,0);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_transf.position,_transf.position+new Vector3((float)0.5,0,0));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_transf.position,_transf.position+new Vector3(0,0,(float)0.5));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_transf.position,_transf.position+new Vector3(0,(float)0.5,0));
        
    }
}
