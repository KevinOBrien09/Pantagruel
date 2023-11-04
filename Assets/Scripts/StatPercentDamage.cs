using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/StatPercentDamage", fileName = "StatPercentDamage")]
public class StatPercentDamage:StatPercentEffect
{
    [Range(0,100)] public int percentage;
    public StatName stat;

    public override void Use(Entity caster,Entity target,bool isPlayer, List<Entity> casterTeam = null ,List<Entity> targetTeam = null)
    {
        target.TakeDamage(Percentage(caster,stat,percentage));
    }

}