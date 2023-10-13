using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ItemDragBehaviour : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IEndDragHandler,IDragHandler,IInitializePotentialDragHandler
{
    public static bool dragging;
    public RectTransform rt;
    public bool interactable;
    public ItemStack stack;
    Canvas parentCanvas;
    Vector2 ogPos;
    List<Tween> activeTweens = new List<Tween>();

    void Start()
    {
        parentCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
        ogPos = rt.anchoredPosition;
    }
    

  
    public void OnBeginDrag(PointerEventData eventData)
    {
      

        if(!interactable)
        { return;}

        if(eventData.button == PointerEventData.InputButton.Left)
        {
            dragging = true;
            stack.Remove();
            stack.CheckIfZero();
        
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!interactable)
        { return;}

        if(gameObject != null)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                stack.Add();
                activeTweens.Add( rt.DOAnchorPos(ogPos,.2f).OnComplete(()=>
                {stack.CheckIfOtherPictureWasDisabled();}));
                
                dragging = false;
            }
        }    
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if(!interactable)
        { return;}

        if(eventData.button == PointerEventData.InputButton.Left)
        {rt.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;}
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!interactable)
        { return;}

        if(eventData.button == PointerEventData.InputButton.Right)
        {
           
        }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    { eventData.useDragThreshold = false; }

    void KillTweens()
    {
        foreach (var item in activeTweens)
        {item.Kill();}
    }

    public void Reset()
    {
        KillTweens();
        rt.DOAnchorPos(ogPos,0);

    }
}
