using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/StatPercentDamage", fileName = "StatPercentDamage")]
public class StatPercentDamage:StatPercentEffect
{
    [Range(0,100)] public int percentage;
    public StatName stat;

    public override void Use(EffectArgs args)
    {
       args. target.TakeDamage(Percentage(args.caster,stat,percentage));
    }

}