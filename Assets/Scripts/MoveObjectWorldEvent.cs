using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveObjectWorldEvent : WorldEvent
{
    public GameObject objectToMove;
    public Vector3 endPos;
    public float time = 1;
    public override void Go(){
        if(!objectToMove.activeSelf){
            objectToMove.SetActive(true);
        }
        objectToMove.transform.DOMove(endPos,time);
    }
}