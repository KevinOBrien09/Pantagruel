using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LookAt : MonoBehaviour
{
    
    public Transform cam;
    public string cameraName;
    
	void Start(){
		cam = GameObject.Find(cameraName).transform;
	}
    
    void LateUpdate()
    {
        transform.LookAt(cam);
      //  transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }

    
}