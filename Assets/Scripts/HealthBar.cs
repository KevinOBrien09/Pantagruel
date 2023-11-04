using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
public class HealthBar : MonoBehaviour
{
    public Entity entity;
    public Image fill;
    public bool healthTxtOneString,onlyShowCurrent;
    public TextMeshProUGUI current,max;
    public UnityEvent onHit;
    public UnityEvent onHeal;
    public UnityEvent onInit;

    public void UpdateFill()
    {
        fill.DOFillAmount(entity.currentHealth/entity.stats().maxHealth,.2f);
    }

    public void UpdateText()
    {
        if(healthTxtOneString)
        {
            current.text = "HP:" + entity.currentHealth.ToString()+"/"+entity.stats().maxHealth.ToString();
        //    max.text = "";
        }
        else if(onlyShowCurrent){
             current.text = entity.currentHealth.ToString();
        }
        else
        {
            current.text = entity.currentHealth.ToString();
            max.text = entity.stats().maxHealth.ToString();
       
        }
      
    }

       


}