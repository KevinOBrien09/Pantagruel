using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{
    public Transform target;
    [SerializeField] float smoothSpeed;
    public Vector3 offset;


    void LateUpdate()
    {
        if(target != null){
            Follow();
        }
        
    }
    
    void Follow()
    {
        Vector3 d = new Vector3 (target.position.x + offset.x,offset.y, target.position.z +  offset.z); 
        Vector3 smooth = Vector3.Lerp(transform.position,d,smoothSpeed);
        transform.position = smooth;
    }
}
 