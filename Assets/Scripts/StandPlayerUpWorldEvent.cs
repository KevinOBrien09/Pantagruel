using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StandPlayerUpWorldEvent : WorldEvent
{

   
    public override void Go(){
        Transform cameraHolder = GameObject.FindGameObjectWithTag("CameraHolder").transform;
        cameraHolder.DOLocalRotate(new Vector3(0,0,0),.25f);
        cameraHolder.DOLocalMoveY(1,.25f);
     
    }

 

}