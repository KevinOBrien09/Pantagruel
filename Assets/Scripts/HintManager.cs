using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using KoganeUnityLib;
using TMPro;
using DG.Tweening;

public class HintManager : Singleton<HintManager>
{
    public TMP_Typewriter typewriter;
    public TextMeshProUGUI text;
    public bool typing;


    void Start()
    {
        text.text ="";
        text.DOFade(0,0);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.P)){
            ShowHint("The beast is calm...");
        }
    }

    public void ShowHint(string hint)
    {
        if(!typing)
        {
            text.DOFade(1,.2f);
            CardManager.inst.DeactivateHand();
            typing = true;
            typewriter.Play(hint,30,()=>{StartCoroutine(q());});

            IEnumerator q()
            {
                yield return new WaitForSeconds(1);
                text.DOFade(0,.2f).OnComplete(()=> text.text = "");
                yield return new WaitForSeconds(.2f);
                CardManager.inst.ActivateHand();
                typing = false;
            }
        }
        
      
    }
}