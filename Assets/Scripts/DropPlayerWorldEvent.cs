using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DropPlayerWorldEvent : WorldEvent
{

   
    public override void Go(){
        Transform cameraHolder = GameObject.FindGameObjectWithTag("CameraHolder").transform;
        cameraHolder.DOLocalRotate(new Vector3(-35,0,-10),.25f);
        cameraHolder.DOLocalMoveY(0,.25f);
     
    }

 

}