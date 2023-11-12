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
            BattleManager.inst.howMuchDamagePlayerDidPerTurn[BattleManager.inst.turn] += (int) PetManager.inst.playerPet.currentHealth;
            PetManager.inst.playerPet.Die(EntityOwnership.PLAYER);
        }
        else
        {
            BattleManager.inst.howMuchDamageEnemyDidPerTurn[BattleManager.inst.turn] += (int) PetManager.inst.enemyPet.currentHealth;
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