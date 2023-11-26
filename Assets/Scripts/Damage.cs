using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Damage", fileName = "Damage")]
public class Damage :Effect
{
   
    public float damageValue;
    public bool negateShield;
    public override void Use(EffectArgs args)
    {
        foreach (var item in AffectedEntities(args))
        {
            (float,Color textColor) q = GetDamageValue(args,item);
            item.TakeDamage(q.Item1,args,negateShield,q.textColor);
        }
    }

    public virtual (float dmg,Color textColor) GetDamageValue(EffectArgs args,Entity target){
        return (damageValue,Color.white);
    }

}