using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/SwapBeast", fileName = "Swap")]
public class SwapBeastEffect :Effect
{
    public override void Use(EffectArgs args)
    {
        if(args.isPlayer){
            List<Beast> b = new List<Beast>(PlayerParty.inst.party);
            b.Remove(PlayerParty.inst.activeBeast);
        
            BeastTargeter.inst.Open(b);
        }  
        else{
            Debug.LogAssertion("make enemy swap");
        }
        
      
   
    }

    public override bool canUse(bool isPlayer)
    {
        if(PlayerParty.inst.party.Count < 2)
        {return false;}
        else
        {return true ;}
    }

}