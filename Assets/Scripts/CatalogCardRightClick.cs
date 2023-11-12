using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class CatalogCardRightClick : MonoBehaviour, IPointerClickHandler
{
    public Card card;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right){
            CardViewer.inst.Open(card);
        }
       
    }
}