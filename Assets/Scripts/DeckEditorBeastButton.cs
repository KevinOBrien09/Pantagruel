using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
public class DeckEditorBeastButton : MonoBehaviour,IPointerClickHandler
{
    public RectTransform rt;
    public Image image;
    public TextMeshProUGUI beastName,beastAtt,deckCostTxt;
    public Button button;
    public Image deckCostFill;
    Beast beast;
    string t = "Tarot: ";

    public void Init(Beast  b)
    {
        BeastScriptableObject bso = b.scriptableObject;
        beast = b;
        image.sprite = bso.beastData.uiPicture;
        if(!bso.beastData.facingRight)
        { image.transform.rotation = Quaternion.Euler(0,-180,0); }
        beastName.text = bso.beastData.beastName;
        beastAtt.text = bso.beastData.element.ToString() + "/" + bso.beastData.beastClass.ToString();
        if( bso.beastData.secondaryClass != BeastClass.COMMON)
        { beastAtt.text += "/" +  bso.beastData.secondaryClass.ToString(); }
        deckCostTxt.text = t + beast.deck.TotalDeckCost().ToString() + "/" + "100";
        deckCostFill.DOFillAmount((float)beast.deck.TotalDeckCost()/100,0 );
    }

    void UpdateDeckCostMeter()
    {
        deckCostTxt.text = t + beast.deck.TotalDeckCost().ToString() + "/" + "100";
        deckCostFill.DOFillAmount((float)beast.deck.TotalDeckCost()/100,.2f );
    }
    public void Click(){
    DeckEditor.inst.BeastSubMenu(beast);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right ){
            Debug.Log("right click");
        }
    }
}