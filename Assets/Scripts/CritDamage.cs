using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/CritDamage", fileName = "CritDamage")]
public class CritDamage :Effect
{
    public float damageValue;
    [Range(0,100)] public int critChance;
    [Range(0,100)] public int luckPercentage;
    public override void Use(Entity caster,Entity target,bool isPlayer, List<Entity> casterTeam = null ,List<Entity> targetTeam = null)
    {
        int cap = 100 -(int)caster.stats().luck;
      

       
        int c = (int) Random.Range(0,100);
        int q = critChance + (int) Maths.Percent( caster.stats().luck , luckPercentage);  
        if(c <= q)
        {target.TakeDamage(damageValue * 2f);
        Debug.Log("Critical");}
        else
        {target.TakeDamage(damageValue);}

       
    }

}