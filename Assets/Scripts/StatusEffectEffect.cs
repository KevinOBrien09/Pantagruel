using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/StatusEffect", fileName = "SE")]
public class StatusEffectEffect :Effect
{
    public StatusEffects statusEffect;
    public int howManyTurns;
    public bool triggerOnceOnCast,triggerOncePerTurn;

    public override void Use(EffectArgs args)
    {
        CreateStatusEffectStack(args);
    }

    public void CreateStatusEffectStack(EffectArgs args){
        if(args.target.statusEffectHandler != null){
            args.target.statusEffectHandler.CreateStack(this,args);
        }
    }

    public virtual void Trigger(Beast infected,EffectArgs args){
Debug.Log(statusEffect.ToString());
    }

}