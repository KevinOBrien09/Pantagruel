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
    public Image fill,shieldFill;
    public bool healthTxtOneString,onlyShowCurrent;
    public TextMeshProUGUI current,max;
    public UnityEvent onHit;
    public UnityEvent onHeal;
    public UnityEvent onInit;

    public void UpdateFill()
    {
        fill.DOFillAmount(entity.currentHealth/entity.stats().maxHealth,.2f);
        shieldFill.DOFillAmount(entity.totalShield()/entity.stats().maxHealth,.2f);

        if( entity.currentHealth < entity.totalShield()  ){
            shieldFill.DOFade(.5f,.2f);
        }
        else{
               shieldFill.DOFade(1,.2f);
        }
    }

    public void UpdateText()
    {
        
        float totalValue = entity.currentHealth + entity.totalShield();
        bool bold = entity.totalShield() == 0;  
       
        if(healthTxtOneString)
        {
            if(!bold)
            {
                current.text = "HP:" +  totalValue.ToString()+"/"+entity.stats().maxHealth.ToString();
            }
            else
            {current.text = "HP:" + "<b>" + totalValue.ToString() + "</b>" +"/"+entity.stats().maxHealth.ToString();}
        }
        else if(onlyShowCurrent)
        {  
            if(!bold)
            {current.text =  totalValue.ToString();}
            else
            {current.text = "<b>"  + totalValue.ToString();}
        }
        else
        {
            if(!bold)
            {
                  current.text = "<b>"  + entity.totalShield().ToString();
                
                max.text = entity.currentHealth.ToString() +  "/" + entity.stats().maxHealth.ToString();
            }
            else
            {
                current.text =  entity.currentHealth .ToString();
                max.text = entity.stats().maxHealth.ToString();

              
            }
       
        }
      
    }

       


}