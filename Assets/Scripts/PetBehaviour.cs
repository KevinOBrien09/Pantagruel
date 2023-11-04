
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PetBehaviour:Entity               
{
    public Pet pet;
    EntityOwnership ownership;

    public void Init(Pet p,EntityOwnership o)
    {
        pet = p;
        ownership = o;
        BeastAnimatedInstance i = Instantiate(pet.animatedInstance,transform);  
        i.Init(this);
        currentHealth = pet.stats.maxHealth;
        animatedInstance = i;
        if(ownership == EntityOwnership.PLAYER){
            gameObject.layer = 8;
        }
        else{
            gameObject.layer = 7;
        }
    }

    public override void Die()
    {
        base.Die();

        AudioManager.inst.GetSoundEffect().Play(pet.dying);
        PetManager.inst.KillPet(this);
        

    }

    public override Stats stats()
    {
        return pet.stats;
    }

}