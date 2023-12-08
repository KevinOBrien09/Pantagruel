using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public enum CameraState{NORMAL,DOWN,UP,LEFT,RIGHT}
public class CameraManager : Singleton<CameraManager>
{
    public CameraState currentState;
    public GameObject CUBE;
    public void ChangeCameraState(CameraState newState)
    {
        if(currentState == newState)
        {return;}

CUBE.SetActive(false);
        float rotSpeed = .25f;
        switch (newState)
        {
            case CameraState.NORMAL:
            transform.DOLocalRotate(new Vector3(0,0,0),rotSpeed).OnComplete(()=> CUBE.SetActive(true));
            break;
            case CameraState.DOWN:
            transform.DOLocalRotate(new Vector3(25,0,0),rotSpeed);
            break;
            case CameraState.UP:
            transform.DOLocalRotate(new Vector3(-25,0,0),rotSpeed);
            break;
            case CameraState.RIGHT:
            transform.DOLocalRotate(new Vector3(0,25,0),rotSpeed);
            break;
            case CameraState.LEFT:
            transform.DOLocalRotate(new Vector3(0,-25,0),rotSpeed);
            break;
            default:
            Debug.LogAssertion("DEFAULT CASE.");
            break;
        }

        currentState = newState;
    }

}