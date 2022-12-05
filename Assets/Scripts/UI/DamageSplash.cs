using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DamageSplash : MonoBehaviour
{
    [SerializeField] Image blood;
    [SerializeField] GameObject cam;
    public const float shakeTime = .5f;
    [SerializeField] RectTransform viewPortHolder;
    RectTransform rt;

    void Start(){
        rt = GetComponent<RectTransform>();

    
    }

    public void Shake()
    {
        MainCameraManager.inst.Shake();
        //viewPortHolder.DOShakeAnchorPos(shakeTime,40,50);
        rt.DOShakeAnchorPos(shakeTime,40,50);
        cam.transform.DOShakePosition(shakeTime,1,30);
        StartCoroutine(Blood());
    }

    public void SoloBlood(){
        blood.DOKill();
           StartCoroutine(Blood());
    }

    IEnumerator Blood()
    {
        blood.DOFade(1,0f);
        yield return new WaitForSeconds(.2f);
        blood.DOFade(0,2f);
        yield return null;
    }
   
}
