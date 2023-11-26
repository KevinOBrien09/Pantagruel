using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/StatPercentDamage", fileName = "StatPercentDamage")]
public class StatPercentDamage:Damage
{
    [Range(0,100)] public int percentage;
    public StatName stat;

    public override (float dmg,Color textColor) GetDamageValue(EffectArgs args, Entity target)
    {
        Color c = new Color();
        switch(stat)
        {
            case StatName.PHYSICAL:
            ColorUtility.TryParseHtmlString("#FF8000",out c);
            break;
            case StatName.MAGIC:
            ColorUtility.TryParseHtmlString("#0060FF",out c);
            break;
            default :
            c = Color.white;
            break;
        }
        return (Percentage(args.caster,stat,percentage),c); 
    }

}