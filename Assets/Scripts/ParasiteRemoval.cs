using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "Effects/ParasiteRemoval", fileName = "ParasiteRemoval")]
public class  ParasiteRemoval : PercentHealthDamage
{
    public override void Use(EffectArgs args)
    {
        base.Use(args);
        args.caster.animatedInstance.Bleed();
        args.caster.statusEffectHandler.RemoveParasiteStack();
      //  args.caster.statusEffectHandler.
    }

    public override bool canUse(bool isPlayer)
    {
       
        if(isPlayer)
        {
            if(PlayerParty.inst.activeBeast.statusEffectHandler.ContainsThisEffect(StatusEffects.PARASITE))
            {return true;}
        }
        else
        {
            if(RivalBeastManager.inst.activeBeast.statusEffectHandler.ContainsThisEffect(StatusEffects.PARASITE))
            {return true;}
        }
        Debug.LogAssertion("PARASITE CARD IN DECK BUT NO PARASITE STACK FOUND!!");
        return false;
    }

}