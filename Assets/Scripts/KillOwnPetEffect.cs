using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/KillOwnPet", fileName = "KillOwnPet")]
public class KillOwnPetEffect :Effect
{
    public override void Use(EffectArgs args)
    {
        if(args.isPlayer)
        {
            BattleManager.TurnRecord.CardIntPair p = new BattleManager.TurnRecord.CardIntPair();
            p.card = args.card;
            p.v = (int) PetManager.inst.playerPet.currentHealth;
             p.castOrder = args.castOrder;
            BattleManager.inst.playerRecord[BattleManager.inst.turn].damageDealtByEachCard.Add(p);
          
            PetManager.inst.playerPet.Die(EntityOwnership.PLAYER);
        }
        else
        {
            BattleManager.TurnRecord.CardIntPair p = new BattleManager.TurnRecord.CardIntPair();
            p.card = args.card;
            p.v = (int) PetManager.inst.playerPet.currentHealth;
            p.castOrder = args.castOrder;
            BattleManager.inst.enemyRecord[BattleManager.inst.turn].damageDealtByEachCard.Add(p);
            
            PetManager.inst.enemyPet.Die(EntityOwnership.ENEMY);
        }
      
       
    }

    public override bool canUse(bool isPlayer)
    {
        if(isPlayer){
            if(PetManager.inst.playerPet!=null){
                return true;
            }
        }
        else{
            if(PetManager.inst.enemyPet!= null){
                return true;
            }
        }

        return false;
    }

}