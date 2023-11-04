using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/SwapBeast", fileName = "Swap")]
public class SwapBeastEffect :Effect
{
    public override void Use(Entity caster,Entity target,bool isPlayer, List<Entity> casterTeam = null ,List<Entity> targetTeam = null)
    {
        List<Beast> b = new List<Beast>(PlayerParty.inst.party);
        b.Remove(PlayerParty.inst.activeBeast);
       
        BeastTargeter.inst.Open(b);
       //target.TakeDamage(damageValue);
    }

    public override bool canUse(bool isPlayer)
    {
        if(PlayerParty.inst.party.Count < 2)
        {return false;}
        else
        {return true ;}
    }

}