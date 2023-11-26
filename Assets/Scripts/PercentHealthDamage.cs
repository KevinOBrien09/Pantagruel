using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "Effects/PercentHealthDamage", fileName = "PercentHealthDamage")]
public class PercentHealthDamage : Damage
{
    [Range(0,100)]public int percent;
    public bool maxHealth,missingHealth,currentHealth;
    public override (float dmg,Color textColor) GetDamageValue(EffectArgs args,Entity target)
    {
        int v = 0;
        if(maxHealth)
        {
            v = (int) Maths.Percent(target.stats().maxHealth,percent);
            Debug.Log(percent+"% of " +target.stats().maxHealth + " is " + v);
        }
        else if(missingHealth)
        {
            double missingHP= target.stats().maxHealth-target.currentHealth;
            v = (int) Maths.Percent(missingHP,percent);
            Debug.Log(percent+"% of " +missingHP + " is " + v);
        }
        else if(currentHealth){
            v = (int) Maths.Percent(target.currentHealth,percent);
            Debug.Log(percent+"% of " +target.currentHealth + " is " + v);
        }
        return (v,Color.white);
    }

}
