using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Wound", fileName = "Wound")]
public class WoundEffect : StatusEffectEffect
{
  

    public override void Use(EffectArgs args)
    {
       
      
        CreateStatusEffectStack(args.target);
            
            //check for dodge
          
        
         
    }
    
     
    
    
}