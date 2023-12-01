using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Stun", fileName = "Stun")]
public class StunEffect :Effect
{


    public override void Use(EffectArgs args)
    {
        
            args.target.Stun();
        
        
      
    }

}