using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/GuardState", fileName = "GuardState")]
public class ChangeGuardStateEffect :StatusEffectEffect
{
    public Entity.GuardState state;
   

    public override void Use(EffectArgs args)
    {
       
            if(args.target.statusEffectHandler != null)
            {args.target.AddShield(args.target.statusEffectHandler.CreateStack(StatusEffects.SHIELD));}
            else
            {Debug.LogWarning(args.target.name + " has no status effect handler cannot add shield");}
        
    }

}

public class Shield{
    public StatusEffectDisplay display;
    public float value;
}