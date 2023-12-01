using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Parasite", fileName = "Parasite")]
public class ParasiteEffect : StatusEffectEffect
{
    public const    string parasiteCard = "58bb91a1-3cf8-4ade-b3b4-5dff8aa4a16d";

    public override void Use(EffectArgs args)
    {
        
            CreateStatusEffectStack(args.target).AddStatusEffectCardToDeck(cards);
        
       
    }
     
    
    
}