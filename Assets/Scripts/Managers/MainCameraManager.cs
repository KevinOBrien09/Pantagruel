using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EZCameraShake;

public class MainCameraManager : Singleton<MainCameraManager >
{
    [SerializeField] Camera c;
    [SerializeField] Transform t;
    [SerializeField] float shakeInst,shakeRough,shakeTime;
    
    

    public void Shake(){
    CameraShaker.Instance.ShakeOnce(shakeInst, shakeRough, shakeTime/2, shakeTime/2);
    }

    public void ChangeFOV(float newFOV)
    {
      
        c.DOFieldOfView(newFOV,.3f);
    }
}
