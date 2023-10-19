using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardCatagoryTicker : MonoBehaviour
{
    public TextMeshProUGUI cataName;
    public Image toggle;
    public BeastClass beastClass;
    public Button button;
    public Color32 grey;
    public string classString;
    public bool greyedOut;

    public void Init(BeastClass b)
    {
        classString = b.ToString();
        cataName.text = classString;
        greyedOut = false;
        beastClass = b;
       
        toggle.enabled = true;
    }
    
    public void GreyOut()
    {
        cataName.text = "<i>" + cataName.text;
        cataName.color = grey;
        button.interactable = false;
        toggle.enabled = false;
        greyedOut = true;
    }

    public void UnGreyOut()
    {
        cataName.text = beastClass.ToString();
        cataName.color = Color.white;
        button.interactable = true;
        toggle.enabled = true;
        greyedOut = false;
    }

    public void Toggle()
    {
   
    }

}