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

}