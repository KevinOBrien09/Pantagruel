using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovePlayerInLocation: Interactable
{
    public CameraState cameraState;
    public Transform goal;
    public float travelSpeed = 1;
    public override void Go()
    {
        OverworldMovement.canMove = false;
        LocationManager.inst.  blackFade.DOFade(1,.25f).OnComplete(()=>
        {
            PlayerManager.inst.movement.transform.position = goal.position;
            PlayerManager.inst.movement.transform.rotation = goal.rotation;
            PlayerManager.inst.movement.rotate.InitRotation(NESW.inst.GetDirection(PlayerManager.inst.movement.rotate.transform));
            StartCoroutine(q());
            IEnumerator q()
            {
                yield return new WaitForSeconds(.2f);
                LocationManager.inst.blackFade.DOFade(0,.25f).OnComplete(()=>{  OverworldMovement.canMove =  true;  Interactor.inst.canInteract = true;});
            }

        });
        // CameraManager.inst.ChangeCameraState(cameraState);
        // Vector3 v =  goal.position;
        // Vector3 p = new Vector3();
        // if(cameraState == CameraState.DOWN)
        // {
        //     Vector3 pl =   PlayerManager.inst.movement.transform.position;
        //     PlayerManager.inst.movement.transform.DOMoveY(pl.y+pl.y/3,0);
        //     p = v;
        // }
        // if(cameraState == CameraState.UP)
        // {
        //     p =    new Vector3(v.x, v.y+v.y/3,v.z);
        // }

        // PlayerManager.inst.movement.gameObject.transform.DOMove(p,travelSpeed).OnComplete(()=>
        // {
        //     OverworldMovement.canMove = true;
        //     CameraManager.inst.ChangeCameraState(CameraState.NORMAL);
        //   
        //     if(cameraState == CameraState.UP){
        //     PlayerManager.inst.movement.transform.DOMoveY(v.y,0);
        //     }
           
           
        // });
    }
}