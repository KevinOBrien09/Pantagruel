using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;
public class HealthBar : MonoBehaviour
{
    public Beast beast;
    public Image fill;
    public bool healthTxtOneString;
    public TextMeshProUGUI current,max;
    public UnityEvent onHit;
    public UnityEvent onHeal;
    public UnityEvent onInit;

    public void UpdateFill()
    {
        fill.DOFillAmount(beast.currentHealth/beast.scriptableObject.beastData.stats.maxHealth,.2f);
    }

    public void UpdateText()
    {
        if(healthTxtOneString)
        {
            current.text = beast.currentHealth.ToString()+"/"+beast.scriptableObject.beastData.stats.maxHealth.ToString();
            max.text = "";
        }
        else{
            current.text = beast.currentHealth.ToString();
            max.text = beast.scriptableObject.beastData.stats.maxHealth.ToString();
       
        }
      
    }

    public void Wipe(){
       

    }

}