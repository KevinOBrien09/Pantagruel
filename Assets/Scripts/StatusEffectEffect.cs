using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/StatusEffect", fileName = "SE")]
public class StatusEffectEffect :Effect
{
    public StatusEffects statusEffect;
    public bool removeAtEndOfCombat;
    public int howManyTurns;
    public List<Card> cards = new List<Card>();
    public override void Use(EffectArgs args)
    {
        
            StatusEffectDisplay d =  CreateStatusEffectStack(args.target);
        

    }

    public StatusEffectDisplay CreateStatusEffectStack(Entity e)
	{
        if(e.statusEffectHandler != null)
		{
            // if(!e.dodged && dodgeable){ //check dodge
                return e.statusEffectHandler.CreateStack(statusEffect);
           // }
            
          
        }
        return null;
    }

    public virtual void Trigger(Beast infected,bool isPlayer){
		Debug.Log(statusEffect.ToString());
    }

}