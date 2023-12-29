using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
 public   bool keepRotation;
    void Awake(){
        transform.SetParent(null);
        transform.position = new Vector3(transform.position.x,5,transform.position.z) ;
        if(keepRotation){
transform.rotation = Quaternion.Euler(-90,transform.rotation.eulerAngles.y,transform.rotation.eulerAngles.z);
        }
        else{
       transform.rotation = Quaternion.Euler(-90,0,0);
        }
 
    }
}
