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
       // ;
        args.caster.AddShield(args.caster.statusEffectHandler.CreateStack(StatusEffects.SHIELD));
       // ChangeGuardState(state);
    }

}

public class Shield{
    public StatusEffectDisplay display;
    public float value;
}