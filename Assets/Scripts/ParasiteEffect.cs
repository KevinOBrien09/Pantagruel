using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Parasite", fileName = "Parasite")]
public class ParasiteEffect : StatusEffectEffect
{
    public Card card;
    
    public override void Trigger(Beast infected,EffectArgs args)
    {
        if(BattleManager.inst.GetBeastOwnership( infected ) == EntityOwnership.PLAYER){
            CardManager.inst.currentDeck.AddCardToDeck(card);
        }
        else
        {
            EnemyAI.inst.currentDeck.AddCardToDeck(card);
        }

      
        
    }
    
}