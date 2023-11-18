using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/RemoveBleed", fileName = "RemoveBleed")]
public class RemoveBleedEffect : Effect
{
    public override void Use(EffectArgs args)
    {
        args.caster.statusEffectHandler.ClearAllBleeds();
    }

    public override bool canUse(bool isPlayer)
    {
        if(isPlayer)
        {
            if(PlayerParty.inst.activeBeast.statusEffectHandler.ContainsABleed())
            {return true;}
        }
        else
        {
            if(RivalBeastManager.inst.activeBeast.statusEffectHandler.ContainsABleed())
            {return true;}
        }
        
        return false;
    }
}