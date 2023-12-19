using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreateVisualTear : WorldEvent
{
    public GameObject[] gosToDisable;
    public float delay;
    public WorldEvent otherEvent;
    
    public override void Go()
    { 
        Image whiteFade = WorldViewManager.inst.whiteFade;
        StartCoroutine (q());
        IEnumerator q()
        {
            yield return new WaitForSeconds(delay);
            whiteFade.DOFade(1,.33f).OnComplete(()=>{
                foreach (var item in gosToDisable)
                {
                    item.gameObject.SetActive(false);
                }
                otherEvent.Go();
                StartCoroutine (q());
                IEnumerator q()
                { yield return new WaitForSeconds(.2f);
                    whiteFade.DOFade(0,.25f);
                }

            });
         
        }
     
    }

}