using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/KillOwnPet", fileName = "KillOwnPet")]
public class KillOwnPetEffect :Effect
{
    public override void Use(EffectArgs args)
    {
        EntityOwnership ownership;
        BattleManager.TurnRecord turnRecord;
        if(args.caster.OwnedByPlayer())
        {
            ownership = EntityOwnership.PLAYER;
            turnRecord =  BattleManager.inst.playerRecord[BattleManager.inst.turn];
            BattleManager.inst.RecordPlayerDamage((int) PetManager.inst.playerPet.currentHealth  );
            PetManager.inst.playerPet.Die(ownership);  
        }
        else
        {
            ownership = EntityOwnership.ENEMY;
            turnRecord =  BattleManager.inst.enemyRecord[BattleManager.inst.turn];
            BattleManager.inst.RecordEnemyDamage((int) PetManager.inst.enemyPet.currentHealth);
            PetManager.inst.enemyPet.Die(ownership);
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