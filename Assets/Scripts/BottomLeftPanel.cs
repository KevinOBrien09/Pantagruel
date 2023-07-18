using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BottomLeftPanel:Singleton<BottomLeftPanel>                   
{
    public GameObject cardStack;
    public GameObject moveArrows;


    public void SwapToArrows(){
        moveArrows.SetActive(true);
        cardStack.SetActive(false);
    }

    public void SwapToStack(){
        moveArrows.SetActive(false);
        cardStack.SetActive(true);
    }
}