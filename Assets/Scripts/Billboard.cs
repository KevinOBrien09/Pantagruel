using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour
{
    Transform cam;
    
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("CameraHolder").transform;
    }

    void LateUpdate()
    {
        transform.LookAt(cam);
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }



    
}