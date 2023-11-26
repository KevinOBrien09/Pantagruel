using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/FlatDrain", fileName = "FlatDrain")]
public class DrainEffect :Damage
{
   
    public override void Use(EffectArgs args)
    {
        float healAmount = 0;
        foreach (var item in AffectedEntities(args))
        {
            (float,Color textColor) q = GetDamageValue(args,item);
            item.TakeDamage(q.Item1,args,negateShield,q.textColor);
            healAmount += q.Item1;
        }
        args.caster.Heal((int) healAmount);
    }

 

}