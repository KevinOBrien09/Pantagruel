using UnityEngine;
using System.Collections.Generic;

public static class StatusEffectFactory
{
    public static StatusEffectInstance Create(Sprite image,int turnDuration,List<StatusEffectData> changeData)
    {
        if(changeData != null)
        {
            StatusEffectInstance holder = new StatusEffectInstance();
            holder.picture = image;
            holder.howManyTurns = turnDuration;
            foreach (var item in changeData)
            {
                if(item.statusEffectType == StatusEffectType.StatMod)
                {
                    StatModification statModData = new StatModification(item.statModData);
                    holder.effects.Add(statModData);
                }
                else if (item.statusEffectType == StatusEffectType.Bleed)
                {
                    Bleed bleed = new Bleed(item.bleedData);
                    holder.effects.Add(bleed);
                }
            }
            return holder;
        }
        else
        {
            return null;
        }
        
    }

}