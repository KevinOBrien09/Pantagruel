using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BattleTextBehaviour : MonoBehaviour
{
    public TextMeshPro t;
    public void Spawn(string text,Color c,bool goLeft)
    {
    
        t.text = text;
        t.color = c;
        transform.DOMoveY(transform.position.y + .25f,.7f).OnComplete(()=>{Destroy(gameObject);});

    //       float x = 0;
    //     if(goLeft){
    //         x = Random.Range(-1,-2); ;
    //     }
    //     else{
    //         x = Random.Range(1,2);
    //     }
       
    //    float y = Random.Range(0,2);
    //     transform.DOLocalMove( new Vector3(x,y,transform.localPosition.z),.7f).OnComplete(()=>{
            
    //         t.DOFade(0,.05F).OnComplete(()=>{
    //         Destroy(gameObject);
    //         });
            
           
    //     });


    }
}