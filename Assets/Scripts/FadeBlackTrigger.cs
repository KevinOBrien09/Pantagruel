using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeBlackTrigger : MonoBehaviour
{

    public void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
           
         LocationManager.inst.blackFade.DOFade(1,.25f);
        }
        
    }
}