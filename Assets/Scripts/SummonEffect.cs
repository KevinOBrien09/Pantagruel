using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Summon", fileName = "Summon")]
public class SummonEffect :Effect
{
    public Pet pet;

    public override void Use(Entity caster,Entity target,bool isPlayer, List<Entity> casterTeam = null ,List<Entity> targetTeam = null)
    {
        if(BattleManager.inst.GetBeastOwnership((Beast) caster) == EntityOwnership.ENEMY)
        {
            PetManager.inst.SummonEnemyPet(pet);
        }
        else if(BattleManager.inst.GetBeastOwnership((Beast) caster) ==EntityOwnership.PLAYER)
        {
            PetManager.inst.SummonPlayerPet(pet);
       
        
        }
        else{Debug.LogAssertion("Uh Oh....");}
    }

    public override bool canUse(bool isPlayer)
    {
        if(isPlayer)
        {
            if(PetManager.inst.playerPet == null){
                return true;
            }
            else{
                return false;
            }
        }
        else{
            if(PetManager.inst.enemyPet == null){
                return true;
            }
            else{
                return false;
            }
        }
    }

}