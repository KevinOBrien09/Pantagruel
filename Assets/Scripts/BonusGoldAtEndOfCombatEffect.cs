using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/BonusGold", fileName = "BonusGold")]
public class BonusGoldAtEndOfCombatEffect :Effect
{
    public int bonusGold;

    public override void Use(EffectArgs args)
    {
        if(args.caster.OwnedByPlayer())
        {RewardManager.inst.additionalGold += bonusGold;} 
        else
        {Debug.LogAssertion("make enemy have reward??");}
      
    }

}