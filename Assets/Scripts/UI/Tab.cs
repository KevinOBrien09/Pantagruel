using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Tab : MonoBehaviour
{
    [SerializeField] RectTransform rt;
    [SerializeField] Button button;
    [SerializeField] Vector2 small;
    [SerializeField] Vector2 big;
    [SerializeField] GameObject panel;
    bool selected;

    public void MakeSmall()
    {
        DOVirtual.Float(rt.sizeDelta.x,small.x,.2f,v  => 
        {rt.sizeDelta = new Vector2(v,rt.sizeDelta.y); });   
        panel.SetActive(false);
        button.image.color = Color.white;
        selected = false;
    }
    
    public void Clicked()
    {
        if(!selected)
        {
            if(SkillDisplay.inst.gameObject.activeSelf)
            {SkillDisplay.inst.StartCoroutine(SkillDisplay.inst.Hide());}
          
            TabManager.inst.DeselectAll();  

            DOVirtual.Float(rt.sizeDelta.x,big.x,.2f,v  => 
            {rt.sizeDelta = new Vector2(v,rt.sizeDelta.y); });   

            panel.SetActive(true);
            button.image.color = Color.red;
            selected = true;
        }
        
    }

}