using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/DrawCard", fileName = "DrawCard")]
public class DrawCardEffect :Effect
{
    public int numberOfCardsToBeDrawn = 1;

    public override void Use(EffectArgs args)
    {
        if(args.isPlayer){
            for (int i = 0; i < numberOfCardsToBeDrawn; i++)
            {
                CardManager.inst.DrawCard();
            }
        }
        else{
            Debug.LogAssertion("make enemy draw card");
        }
        
      
    }

}