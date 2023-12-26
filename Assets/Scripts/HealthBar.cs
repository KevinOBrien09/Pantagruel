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
    public bool healthTxtOneString,onlyShowCurrent,doShieldObject,addHpToString;
    public GameObject shieldObject;
    public TextMeshProUGUI current,max,shieldText;
    public UnityEvent onHit;
    public UnityEvent onHeal;
    public UnityEvent onInit;

    void Start(){
        if(shieldObject != null){
            shieldObject.SetActive(false);
        }
    }

    public void UpdateFill()
    {
        fill.DOFillAmount(entity.currentHealth/entity.stats().maxHealth,.2f);
        shieldFill.DOFillAmount(entity.totalShield()/entity.stats().maxHealth,.2f);
        if(entity.OwnedByPlayer()){
  if( entity.currentHealth < entity.totalShield()  ){
            shieldFill.DOFade(.5f,.2f);
        }
        else{
               shieldFill.DOFade(1,.2f);
        }
        }
      
        if(shieldText!= null){
            if(entity.totalShield() !=0)
            {

                shieldText.text = entity.totalShield().ToString();
            }
            else{
                shieldText.text = "";
            }
        }
        
    }

    public void UpdateText()
    {
        
        float totalValue = entity.currentHealth + entity.totalShield();
        bool bold = false;  
        // entity.totalShield() == 0;  
       
        if(healthTxtOneString)
        {
            if(!bold)
            {
                string hp = "";
                if(addHpToString){
                    hp += "HP:";
                }
                //"HP:" + 
                current.text =  hp+  entity.currentHealth.ToString()+"/"+entity.stats().maxHealth.ToString();
                if(entity.totalShield() !=0)
                {
                    if(doShieldObject){
                        shieldObject.SetActive(true);
                        shieldText.text = entity.totalShield().ToString();
                    }

                }
                else{
                    if(doShieldObject){
                        shieldObject.SetActive(false);
                        shieldText.text = ": " +"0";
                    }
                }
                //totalValue.ToString()
              
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
                if(entity.totalShield() != 0)
                {
                    current.gameObject.SetActive(true);
                    current.text = "<b>"  + entity.totalShield().ToString();

                }
                else{
                    current.gameObject.SetActive(false);
                }
                  
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