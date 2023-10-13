using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class DeckEditorBeastButton : MonoBehaviour,IPointerClickHandler
{
    public RectTransform rt;
    public Image image;
    public TextMeshProUGUI beastName,beastAtt;
    public Button button;
    Beast beast;

    public void Init(Beast  b)
    {
        BeastScriptableObject bso = b.scriptableObject;
        beast = b;
        image.sprite = bso.beastData.uiPicture;
        if(!bso.beastData.facingRight){
            image.transform.rotation = Quaternion.Euler(0,-180,0);
        }
        beastName.text = bso.beastData.beastName;
        beastAtt.text = bso.beastData.element.ToString() + "/" + bso.beastData.beastClass.ToString();
        if( bso.beastData.secondaryClass != BeastClass.COMMON){
            beastAtt.text += "/" +  bso.beastData.secondaryClass.ToString();
        }
    }

    public void Click(){
        DeckEditor.inst.SelectBeast(beast,this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right ){
            Debug.Log("right click");
        }
    }
}