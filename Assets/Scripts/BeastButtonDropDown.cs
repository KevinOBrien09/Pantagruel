using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class BeastButtonDropDown: Singleton<BeastButtonDropDown>,IPointerExitHandler
{ 
    public Camera uiCamera;
    public RectTransform rt;
    DeckEditorBeastButton deckEditorBeastButton;

    void Start(){
        gameObject.SetActive(false);
    }

    public void Move(DeckEditorBeastButton beastButton,  Vector2 mouseClick)
    {
        Camera  c = GameObject.Find("UICamera").GetComponent<Camera>();
        Vector2 v = new Vector2();
        deckEditorBeastButton = beastButton;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.gameObject.GetComponent<RectTransform>(),mouseClick,c,out v);
        rt.anchoredPosition = v;
        
        gameObject.SetActive(true);
            
    }

    public void SwapButton(){
        BeastSwapper.inst.Swap(deckEditorBeastButton.beast);
        DeckEditor.inst.SortBeastButtons();
        Close();
    }

    public void OpenBeastProfile(){
        BeastProfileViewer.inst.Open(deckEditorBeastButton.beast);
       
        Close();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       Close();
    }

    void Close(){
        deckEditorBeastButton = null;
        gameObject.SetActive(false);
    }
}