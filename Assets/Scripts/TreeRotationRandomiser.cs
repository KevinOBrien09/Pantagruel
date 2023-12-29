using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class TreeRotationRandomiser : MonoBehaviour
{
    void Start(){
        Randomize();
    }
   public void Randomize()
    { 
        // if(transform.childCount > 0){
        // transform.GetChild(0).SetParent(null);
        // }
     

        float y =  Random.Range(0f,360);
        transform.rotation = Quaternion.Euler(    
        transform.rotation.eulerAngles.x,y,y);
        float s =  Random.Range(.6f,2f);
        transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y,s);
    
       
        
    }

      
}