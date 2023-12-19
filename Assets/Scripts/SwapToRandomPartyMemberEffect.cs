using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/RandomSwapBeast", fileName = "RandomSwap")]
public class SwapToRandomPartyMemberEffect :Effect
{
    
    public override void Use(EffectArgs args)
    {
        if(args.caster.OwnedByPlayer()){
            List<Beast> b = new List<Beast>(PlayerParty.inst.party);
            b.Remove(PlayerParty.inst.activeBeast);
            int i = Random.Range(0,b.Count);
        
            BeastTargeter.inst.CommenceSwap(b[i]);
        }  
        else{
            Debug.LogAssertion("make enemy swap");
        }
        
      
   
    }

    public override bool canUse(bool isPlayer)
    {
        if(PlayerParty.inst.party.Count == 1)
        {return false;}
        else
        {return true ;}
    }

}