using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/StatModiferEffect", fileName = "StatModiferEffect")]
public class StatModiferEffect : Effect
{
    
    public StatName statName;
    public int amount;
    public int duration;
    public bool untilEndOfCombat;
    public override void Use(EffectArgs args)
    {
        
            StatMod mod = new StatMod();
            mod.change = amount;
            mod.stat = statName;
            mod.turnToDieOn = duration + BattleManager.inst.turn;
            mod.untilEndOfCombat = untilEndOfCombat;
           args.target.ModifyStat(mod);
        
    }
     
    
    
}