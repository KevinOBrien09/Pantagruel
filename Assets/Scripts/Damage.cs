using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Damage", fileName = "Damage")]
public class Damage :Effect
{
   
    public float damageValue;
    public bool negateShield;
    public SoundData hit;
    public Effect additionalEffect;
    public override void Use(EffectArgs args)
    {

       
        
        
          
        (float,Color textColor) q = GetDamageValue(args,args.target);
        args.target.TakeDamage(q.Item1,args,negateShield,q.textColor);
        if(additionalEffect !=null)
        {
            foreach (var item in additionalEffect. AffectedEntities(args))
            {
                EffectArgs ea = new EffectArgs(args.caster,item,args.card,args.stackBehaviour,args.castOrder,args.tickerTitle);
                additionalEffect. Use(ea);
            }
          
        }
       
         //AudioManager.inst.GetSoundEffect().Play(hit);
    }

    public virtual (float dmg,Color textColor) GetDamageValue(EffectArgs args,Entity target){
        return (damageValue,Color.white);
    }

}