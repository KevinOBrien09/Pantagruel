using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/StatusEffect", fileName = "SE")]
public class StatusEffectEffect :Effect
{
    public bool conditional;
    public int howManyTurns;
    public bool stackable;

    public override void Use(EffectArgs args)
    {
        if(args.isPlayer){
            BattleManager.inst.playerStatusEffects.CreateStatusEffect(args.card,this);
        }
    }

}