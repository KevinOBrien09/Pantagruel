using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/DrawCardOfSpecificTrait", fileName = "DrawCardOfSpecificTrait")]
public class DrawCardOfSpecificTrait :Effect
{
    public Element element;
    public BeastClass beastClass;
    public bool lookForElement;
    public override void Use(EffectArgs args)
    {
        if(args.isPlayer)
        {
           
            CardManager.inst.CreateCardBehaviour(CardManager.inst.DrawCardOfSpecificTrait(beastClass)); 
                CardManager.inst.ActivateHand();
                 CardManager.inst.MakeHandInteractable();
          
           
            
        }
        else{
            Debug.LogAssertion("make enemy draw card");
        }
        
      
    }

    public override bool canUse(bool isPlayer)
    {
        return CardManager.inst.currentDeck.deckContainsCardOfTrait(beastClass);
    }

}