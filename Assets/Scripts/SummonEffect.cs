using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Summon", fileName = "Summon")]
public class SummonEffect :Effect
{
    public Pet pet;

    public override void Use(EffectArgs args)
    {
        if(args.caster.OwnedByPlayer())
        { PetManager.inst.SummonPlayerPet(pet);}
        else
        {PetManager.inst.SummonEnemyPet(pet);}
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