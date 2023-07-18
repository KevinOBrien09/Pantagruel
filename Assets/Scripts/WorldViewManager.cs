 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class WorldViewManager:Singleton<WorldViewManager>
{
    public CanvasGroup worldView;
    public Image abstractBG;
    public bool cardHoveringOverViewport;
    public CardBehaviour currentCardOverViewPort;
    public void EnterBattle()
    {
        abstractBG.gameObject.SetActive(true);
        worldView.DOFade(0,.7f);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        CardBehaviour cb = null;
        if(other.gameObject.transform.parent.parent.TryGetComponent<CardBehaviour>(out cb))
        {
            if(CardBehaviour.dragging){
                cardHoveringOverViewport=true;
                currentCardOverViewPort = cb;
            }
          
        }
    }
    public void OnTriggerExit(Collider other)
    {
        CardBehaviour cb = null;
        if(other.gameObject.transform.parent.parent.TryGetComponent<CardBehaviour>(out cb))
        {
            if(CardBehaviour.dragging){
                cardHoveringOverViewport=false;
                currentCardOverViewPort = null;
            }
           
        }
    }

    void Update(){
        if(cardHoveringOverViewport)
        {
            if(Input.GetMouseButtonUp(0))
            {
                if(currentCardOverViewPort.canCast())
                {
                    currentCardOverViewPort.Cast();
                    cardHoveringOverViewport = false;
                    currentCardOverViewPort = null;
                }
                else
                {Debug.Log("Not enough Mana");}
            }
        }
    }

   
}