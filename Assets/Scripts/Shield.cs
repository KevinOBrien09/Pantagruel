using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shield : MonoBehaviour
{
    
    public Transform cam;
    
    
    
    void LateUpdate()
    {
        transform.LookAt(cam);
      //  transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }

    
}