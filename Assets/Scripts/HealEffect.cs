using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Heal", fileName = "Heal")]
public class HealEffect :Effect
{
    public int healAmount;

    public override void Use(Entity caster,Entity target,bool isPlayer, List<Entity> casterTeam = null ,List<Entity> targetTeam = null)
    {
        caster.Heal(healAmount);
      
    }

}