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
    
    public bool cardHoveringOverViewport,itemHoveringOverViewport;
    public CardBehaviour currentCardOverViewPort;
    public  ItemStack currentItemStack;
    public void EnterBattle()
    {
        abstractBG.gameObject.SetActive(true);
        worldView.DOFade(0,.7f);
    }

    public void LeaveBattle()
    {
       
        worldView.DOFade(1,.7f).OnComplete(()=>{ abstractBG.gameObject.SetActive(false);});
    }
    
    
    public void OnTriggerEnter(Collider other)
    {
        CardBehaviour cb = null;
        ItemStack id = null;
        if(other.gameObject.transform.parent.parent.TryGetComponent<CardBehaviour>(out cb))
        {
            if(CardBehaviour.dragging){
                cardHoveringOverViewport=true;
                currentCardOverViewPort = cb;
            }
        }

        if(other.gameObject.transform.parent.TryGetComponent<ItemStack>(out id))
        {
            if(ItemDragBehaviour.dragging)
            {
                itemHoveringOverViewport =true;
                currentItemStack = id;
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        CardBehaviour cb = null;
        ItemStack id = null;
        if(other.gameObject.transform.parent.parent.TryGetComponent<CardBehaviour>(out cb))
        {
            if(CardBehaviour.dragging){
                cardHoveringOverViewport=false;
                currentCardOverViewPort = null;
            }
           
        }

        if(other.gameObject.transform.parent.TryGetComponent<ItemStack>(out id))
        {
            if(ItemDragBehaviour.dragging)
            {
                itemHoveringOverViewport =false;
                currentItemStack = null;
               
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
                {

                    AudioManager.inst.GetSoundEffect().Play(SystemSFX.inst.errorSound);
                    Debug.Log("cannot cast" + currentCardOverViewPort.card.cardName);
                    if(ManaManager.inst.currentMana < currentCardOverViewPort.card.manaCost){
ManaManager.inst.FlashMana();
                    }
                        cardHoveringOverViewport = false;
                    currentCardOverViewPort = null;
                   
                }
            }
        }

        if(itemHoveringOverViewport)
        {
            if(Input.GetMouseButtonUp(0))
            {
                if(currentItemStack.canBeUsed())
                {
                    currentItemStack.Use();
                    itemHoveringOverViewport =false;
                    currentItemStack = null;
                }
            }
        }
    }

   
}