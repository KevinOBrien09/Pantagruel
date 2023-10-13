using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardCatagoryTicker : MonoBehaviour
{
    public TextMeshProUGUI cataName;
    public Image toggle,bg;
    public Element element;
    public BeastClass beastClass;
    public Button button;
    public Color32 grey;

    public void Init(BeastClass b,bool clickable)
    {
        cataName.text = b.ToString();
        beastClass = b;
        button.enabled = clickable;
        toggle.enabled = true;
    }
    
    public void GreyOut()
    {
        cataName.text = "<i>" + cataName.text;
        cataName.color = grey;
        button.interactable = false;
        toggle.enabled = false;
    }

    public void Toggle()
    {
        toggle.enabled = CardCatalog.inst.EditFilter(element,beastClass);
    }

}