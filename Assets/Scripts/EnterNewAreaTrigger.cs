using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnterNewAreaTrigger : MonoBehaviour
{
    public int subLocID;
    public Vector3 rot,pos;
    public void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
           
           LocationManager.inst.ChangeSubLocationWithFade(LocationManager.inst.currentMainLoc.subLocations[subLocID],pos,rot);
           if(DialogManager.inst.inDialog){
DialogManager.inst.Reset();
           }
           
           Destroy(gameObject);
        }
        
    }
}