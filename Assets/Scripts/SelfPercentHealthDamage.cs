using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "Effects/SelfPercentHealthDamage", fileName = "SelfPercentHealthDamage")]
public class SelfPercentHealthDamage : Effect
{
    [Range(0,100)]public int percent;
    public bool maxHealth,missingHealth,currentHealth;
    public override void Use(EffectArgs args)
    {
       
        int damageValue = 0;
        if(maxHealth)
        {
            damageValue = (int) Maths.Percent(args.caster.stats().maxHealth,percent);
            Debug.Log(percent+"% of " +args.caster.stats().maxHealth + " is " + damageValue);
        }
        else if(missingHealth)
        {
            double missingHP= args.caster.stats().maxHealth-args.caster.currentHealth;
            damageValue = (int) Maths.Percent(missingHP,percent);
            Debug.Log(percent+"% of " +missingHP + " is " + damageValue);
        }
        else if(currentHealth){
            damageValue = (int) Maths.Percent(args.caster.currentHealth,percent);
            Debug.Log(percent+"% of " +args.caster.currentHealth + " is " + damageValue);
        }
  
        args.caster.TakeDamage(damageValue,args);
    }

}
