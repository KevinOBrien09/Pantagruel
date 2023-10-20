using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using TMPro;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI txt;
    public string toolTipTxt;
    void Start(){
        txt.DOFade(0,0);
        txt.text = toolTipTxt;
    }
    public void Init(string txt_)
    {
        txt.text = txt_;
        toolTipTxt = txt_;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
       txt.DOFade(1,.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      txt.DOFade(0,.2f);
    }
}
