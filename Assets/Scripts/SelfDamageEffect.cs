using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/SelfDamage", fileName = "SelfDamage")]
public class SelfDamageEffect :Effect
{
    public float damageValue;

    public override void Use(EffectArgs args)
    {
        
      
        args.caster.TakeDamage(damageValue,args);
    }

}