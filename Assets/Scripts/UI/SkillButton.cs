using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillButton : Button
{
    public SkillHolder holder;
    public bool showHover;
    public SoundData hover;
   
    
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if(showHover)
        {
            base.OnPointerEnter(eventData);
            SkillDisplay.inst.Show(holder.data);
            AudioManager.inst.GetSoundEffect().Play(hover);
        }
       
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if(showHover)
        {
            if(SkillDisplay.inst.gameObject.activeInHierarchy)
            {SkillDisplay.inst.c = SkillDisplay.inst.StartCoroutine( SkillDisplay.inst.Hide());}
            
        
            base.OnPointerExit(eventData);
        }
    }


    public void BlackWhite(bool makeBlackAndWhite)
    {
        if(makeBlackAndWhite)
        { 
            Material mat = Instantiate(image.material);
            mat.SetFloat("_EffectAmount", 1f);
            image.material = mat;
        }
        else
        {   
            Material mat = Instantiate(image.material);
            mat.SetFloat("_EffectAmount", 0f);
            image.material = mat;
        }
    }

    





}