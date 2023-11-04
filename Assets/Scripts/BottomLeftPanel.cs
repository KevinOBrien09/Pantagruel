using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BottomLeftPanel:Singleton<BottomLeftPanel>                   
{
    public GameObject cardStack;
    public GameObject moveArrows;

    void Start(){
        SwapToOverworld();
    }


    public void SwapToOverworld(){
        moveArrows.SetActive(true);
        cardStack.SetActive(false);
    }

    public void SwapToBattle(){
        moveArrows.SetActive(false);
        cardStack.SetActive(true);
    }
}