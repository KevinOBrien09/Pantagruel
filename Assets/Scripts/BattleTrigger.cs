using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleTrigger : MonoBehaviour
{
    public Dialog dialog;
    public bool doNotKill;
    public void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
          //  CameraManager.inst.ChangeCameraState(CameraState.DOWN);
            OverworldMovement.canMove = false;
            BattleManager.inst.StartBattle(BattleType.Wild);
            if(!doNotKill){
                Destroy(gameObject.transform.parent.gameObject);
            }
           
        }
        
    }
}