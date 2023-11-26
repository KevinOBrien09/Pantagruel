using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/KillOwnPet", fileName = "KillOwnPet")]
public class KillOwnPetEffect :Effect
{
    public override void Use(EffectArgs args)
    {
        foreach (var item in AffectedEntities(args))
        { 
            EntityOwnership ownership;
            BattleManager.TurnRecord turnRecord;
            if(args.isPlayer)
            {
                ownership = EntityOwnership.PLAYER;
                turnRecord =  BattleManager.inst.playerRecord[BattleManager.inst.turn];
            }
            else
            {
                ownership = EntityOwnership.ENEMY;
                turnRecord =  BattleManager.inst.enemyRecord[BattleManager.inst.turn];
            }

            BattleManager.TurnRecord.CardIntPair p = new BattleManager.TurnRecord.CardIntPair();
            p.card = args.card;
            p.v = (int) PetManager.inst.playerPet.currentHealth;
            p.castOrder = args.castOrder;
            turnRecord.damageDealtByEachCard.Add(p);
            PetManager.inst.playerPet.Die(ownership);
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