using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Heal", fileName = "Heal")]
public class HealEffect :Effect
{
    public int healAmount;

    public override void Use(EffectArgs args)
    {
        args.caster.Heal(healAmount);
      
    }

    public override bool canUse(bool isPlayer)
    {
        if(isPlayer)
        {
            if(PlayerParty.inst.activeBeast.currentHealth != PlayerParty.inst.activeBeast.stats().maxHealth)
            {return true;}
        }
        else
        {
            if((RivalBeastManager .inst.activeBeast.currentHealth != RivalBeastManager .inst.activeBeast.stats().maxHealth))
            {return true;}
        }

        return false;
    }

}