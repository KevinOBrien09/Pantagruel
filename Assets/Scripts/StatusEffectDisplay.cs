using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectDisplay : MonoBehaviour
{
    public StatusEffectHandler statusEffectHandler;
    public List<GameObject> graphics = new List<GameObject>();
    public int turnToDie;
    public int turnCastOn;
    public StatusEffects statusEffect;

    public EffectArgs args;
   public StatusEffectEffect scriptableObject;

    public void Init(StatusEffectEffect so)
    {
        StatusEffects effects = so.statusEffect;

        switch(effects)
        {
            case StatusEffects.BLEED:
            graphics[0].SetActive(true);
            break;
            case StatusEffects.PARASITE:
            graphics[2].SetActive(true);
            break;
            case StatusEffects.HOLYWATER:
            graphics[3].SetActive(true);
            break;
            default:
            Debug.LogAssertion("Default Case");
            break;
        }

        turnCastOn = BattleManager.inst.turn;
        turnToDie = turnCastOn + so.howManyTurns;
        statusEffect = effects;
        scriptableObject = so;
        if( scriptableObject .triggerOnceOnCast)
        {Trigger();}
    }

    public void Trigger(){
        // if(statusEffect == StatusEffects.BLEED){
        //     Debug.Log("Bleed");
        //     return;
        // }
        scriptableObject.Trigger(statusEffectHandler.owner,args);
      
    }

    public void Kill(){
        statusEffectHandler.currentEffects.Remove(statusEffect);
        statusEffectHandler.displays.Remove(this);
        Destroy(this.gameObject);
    }
}