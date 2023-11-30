using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class CatalogCardRightClick : MonoBehaviour, IPointerClickHandler
{
    public Card card;
    public bool manifest;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right){
            if(manifest)
            {
                if(eventData.button == PointerEventData.InputButton.Right)
                {
                    List<Card> cards = new List<Card>();
                    foreach (var item in CardManifester.inst.currentSelection)
                    {
                        cards.Add(item);
                    }
                    
                    
                    CardViewer.inst.manuallyLoadCards = true;
                    CardManager.inst.DeactivateHand();
                    CardViewer.inst.ManuallyLoadCards(cards);
                    CardViewer.inst.Open(card);
                    return;
                    
                }
                
            }



            CardViewer.inst.Open(card);
        }
       
    }
}