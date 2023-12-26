using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/GuardState", fileName = "GuardState")]
public class ChangeGuardStateEffect :StatusEffectEffect
{
    public Entity.GuardState state;
    public int shieldAmount;
   

    public override void Use(EffectArgs args)
    {
        foreach (var item in  AffectedEntities(args))
            {
            if(args.target.statusEffectHandler != null)
            {item.AddShield(shieldAmount, item.statusEffectHandler.CreateStack(StatusEffects.SHIELD));}
            else
            {Debug.LogWarning(args.target.name + " has no status effect handler cannot add shield");}
        }
        
    }

}

public class Shield{
    public StatusEffectDisplay display;
    public float value;
}