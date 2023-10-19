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
    
    }
}