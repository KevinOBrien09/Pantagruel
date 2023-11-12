
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PetBehaviour:Entity               
{
    public Pet pet;
    public EntityOwnership ownership;

    public void Init(Pet p,EntityOwnership o)
    {
        pet = p;
        ownership = o;
        BeastAnimatedInstance i = Instantiate(pet.animatedInstance,transform);  
        i.Init(this);
        currentHealth = pet.stats.maxHealth;
        animatedInstance = i;
        
        if(ownership == EntityOwnership.PLAYER)
        {
            gameObject.layer = 8;
            foreach(Transform child in i.transform)
            {child.gameObject.layer = 8;}
            i.transform.localPosition = p.playerPos;
            i.transform.localScale = p.playerScale;
        }
        else
        {
            gameObject.layer = 7;
            foreach(Transform child in i.transform )
            {child.gameObject.layer = 7;}
            i.transform.localPosition = p.enemyPos;
            i.transform.localScale = p.enemyScale;
        }
    }

    public override void Die(EntityOwnership damageSource)
    {
        base.Die(damageSource);

        AudioManager.inst.GetSoundEffect().Play(pet.dying);
        PetManager.inst.KillPet(this);
        

    }

    public override Stats stats()
    {
        return pet.stats;
    }

}