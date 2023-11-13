using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Damage", fileName = "Damage")]
public class Damage :Effect
{
    public float damageValue;

    public override void Use(EffectArgs args)
    {
        
      
        args.target.TakeDamage(damageValue,args);
    }

}