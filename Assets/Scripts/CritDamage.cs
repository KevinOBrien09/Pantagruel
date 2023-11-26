using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/CritDamage", fileName = "CritDamage")]
public class CritDamage :Damage
{

    [Range(0,100)] public int critChance;
    [Range(0,100)] public int luckPercentage;
    public override (float dmg,Color textColor) GetDamageValue(EffectArgs args,Entity target)
    {
        int cap = 100 -(int)args.caster.stats().luck;
        int c = (int) Random.Range(0,100);
        int q = critChance + (int) Maths.Percent( args.caster.stats().luck , luckPercentage);  
        if(c <= q)
        {
            Debug.Log("Critical");
            return  (damageValue * 2f , Color.white);
        }
        else
        { return (damageValue,Color.white);}

       
    }

}