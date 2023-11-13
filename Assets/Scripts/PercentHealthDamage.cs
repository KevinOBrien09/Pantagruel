using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "Effects/PercentHealthDamage", fileName = "PercentHealthDamage")]
public class PercentHealthDamage : Effect
{
    [Range(0,100)]public int percent;
    public bool maxHealth,missingHealth,currentHealth;
    public override void Use(EffectArgs args)
    {
       
        int damageValue = 0;
        if(maxHealth)
        {
            damageValue = (int) Maths.Percent(args.target.stats().maxHealth,percent);
            Debug.Log(percent+"% of " +args.target.stats().maxHealth + " is " + damageValue);
        }
        else if(missingHealth)
        {
            double missingHP= args.target.stats().maxHealth-args.target.currentHealth;
            damageValue = (int) Maths.Percent(missingHP,percent);
            Debug.Log(percent+"% of " +missingHP + " is " + damageValue);
        }
        else if(currentHealth){
            damageValue = (int) Maths.Percent(args.target.currentHealth,percent);
            Debug.Log(percent+"% of " +args.target.currentHealth + " is " + damageValue);
        }
  
        args.target.TakeDamage(damageValue,args);
    }

}
