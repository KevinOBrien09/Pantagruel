using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageValueGraphic : MonoBehaviour{
    public TextMeshPro text;

    public void Spawn(int value,Color c,bool goLeft){
        text.text = value.ToString();
        text.color = c;
      
        // transform.localPosition = new Vector3(0,y,transform.localPosition.z);
        float x = 0;
        if(goLeft){
            x = Random.Range(-3,-5); ;
        }
        else{
            x = Random.Range(3,5);
        }
       
       float y = Random.Range(0,5);
        transform.DOLocalMove( new Vector3(x,y,transform.localPosition.z),.7f).OnComplete(()=>{
            
            text.DOFade(0,.05F).OnComplete(()=>{
            Destroy(gameObject);
            });
            
           
        });
    }
}