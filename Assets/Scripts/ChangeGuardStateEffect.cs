using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/GuardState", fileName = "GuardState")]
public class ChangeGuardStateEffect :Effect
{
    public Entity.GuardState state;

    public override void Use(EffectArgs args)
    {
        
      
        args.target.ChangeGuardState(state);
    }

}