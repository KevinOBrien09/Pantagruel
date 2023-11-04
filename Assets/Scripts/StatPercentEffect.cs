using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/CustomDamage", fileName = "CustomDamage")]
public class StatPercentEffect :Effect
{
  
    public int Percentage(Entity b, StatName s,int percentage)
    {
        return  (int) Maths.Percent(b.stats().GetStat(s),percentage);
  

    }
  

}