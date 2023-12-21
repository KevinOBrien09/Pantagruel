using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveObjectWorldEvent : WorldEvent
{
    public GameObject objectToMove;
    public Vector3 endPos;
    public bool local;
    public float time = 1;
    public override void Go(){
        if(!objectToMove.activeSelf){
            objectToMove.SetActive(true);
        }
        if(!local){
   objectToMove.transform.DOMove(endPos,time);
        }
        else{
               objectToMove.transform.DOLocalMove(endPos,time);
        }
     
    }

    [ContextMenu("Get Pos")]
    void DoSomething()
    {
        if(objectToMove != null){
            endPos = objectToMove.transform.position;
        }
    }

}