using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro ;
using DG.Tweening;

public class BeastProfileViewer : Singleton<BeastProfileViewer>
{ 
    public Image beastPicture,beastShadow,hpFill,expFill,elementBG,mainClassIcon,seconaryClassIcon;
    public TextMeshProUGUI beastName,beastPassiveName,beastPassiveDesc,hpTxt,classification;
    public Transform beastImageHolder;

    void Start(){
        gameObject.SetActive(false);
    }

   public void Open(Beast b){
        Apply(b);
        gameObject.SetActive(true);
    }

    public void Leave(){
        gameObject.SetActive(false);
    }

    public void Apply(Beast b){
        BeastData bd = b.scriptableObject.beastData;
        beastName.text = bd.beastName;
        beastPicture.sprite = bd.mainSprite;
        beastShadow.sprite = bd.mainSprite;
        if(bd.facingRight)
        {
            beastImageHolder.rotation = Quaternion.Euler(0,0,0);
        }
        else{
            beastImageHolder.rotation = Quaternion.Euler(0,180,0);
        }
        hpFill.DOFillAmount((float)b.currentHealth/(float)bd.stats.maxHealth,.5f);
        hpTxt.text = "HP:" + b.currentHealth.ToString() + "/" + bd.stats.maxHealth;
        string s = bd.element.ToString()+"/"+bd.beastClass.ToString();
        elementBG.color = BeastBank.inst.GetElementColour(bd.element);
        mainClassIcon.sprite = BeastBank.inst.GetBeastClassSprite(bd.beastClass);
       
        if(bd.secondaryClass != BeastClass.COMMON)
        {
            seconaryClassIcon.gameObject.SetActive(true);
            seconaryClassIcon.sprite = BeastBank.inst.GetBeastClassSprite(bd.secondaryClass);
            s+= "/" + bd.secondaryClass.ToString();
        }
        else{
              seconaryClassIcon.gameObject.SetActive(false);
        }
       classification.text = s;
        if(bd.passive != null)
        {
            beastPassiveName.text = bd.passive.passiveName;
            beastPassiveDesc.text = bd.passive.passiveDesc;
        }
        else{
             beastPassiveName.text = "Passive: Empty";
            beastPassiveDesc.text = "Not implemented.";
        }
        

    }

}