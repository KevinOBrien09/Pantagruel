using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Damage", fileName = "Damage")]
public class Damage :Effect
{
    public float damageValue;

    public override void Use(Entity caster,Entity target,bool isPlayer, List<Entity> casterTeam = null ,List<Entity> targetTeam = null)
    {
       target.TakeDamage(damageValue);
    }

}