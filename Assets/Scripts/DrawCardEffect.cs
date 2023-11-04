using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/DrawCard", fileName = "DrawCard")]
public class DrawCardEffect :Effect
{
    public int numberOfCardsToBeDrawn = 1;

    public override void Use(Entity caster,Entity target,bool isPlayer, List<Entity> casterTeam = null ,List<Entity> targetTeam = null)
    {
        for (int i = 0; i < numberOfCardsToBeDrawn; i++)
        {
            CardManager.inst.DrawCard();
        }
      
    }

}