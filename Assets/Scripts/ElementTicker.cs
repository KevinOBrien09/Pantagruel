using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ElementTicker : MonoBehaviour
{
    public Element element;
    public Image toggle;
    public TextMeshProUGUI txt;
    public Button button;
    public Color32 grey;

    public void Init(Element e){
            txt.text = e.ToString();
            element = e;
    }
    
    public void Click()
    {
       toggle.enabled = DeckEditor.inst.FilterElement(element);
    }
    
    public void GreyOut()
    {
         txt.text = "<i>" +  txt.text;
         txt.color = grey;
        button.interactable = false;
        toggle.enabled = false;

    }

    public void UnGreyOut()
    {
        string s = element.ToString();
        if(element == Element.NONE){
            s = "UNELEMENTAL";
        }
        txt.text =  s;
        txt.color = Color.white;
        button.interactable = true;
        toggle.enabled = true;

    }
}