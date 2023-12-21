using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateObjectWorldEvent : WorldEvent
{
    public GameObject objectToMove;
    public Vector3 endRot;
    public float time = 1;
    public override void Go(){
        if(!objectToMove.activeSelf){
            objectToMove.SetActive(true);
        }
        objectToMove.transform.DOLocalRotate(endRot,time);
    }

    [ContextMenu("Get Pos")]
    void DoSomething()
    {
        if(objectToMove != null){
            endRot = objectToMove.transform.rotation.eulerAngles;
        }
    }

}