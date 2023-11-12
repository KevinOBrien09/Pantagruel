 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{    
    public List< HealthBar> currentHealthBars = new List<HealthBar>();
    public BeastAnimatedInstance animatedInstance;
    public SoundData healSFX;
    public float currentHealth;
    public bool KO;
    public void TakeDamage(float amount,EntityOwnership damageSource)
    {
        float oldCurrentHealth = currentHealth;
        currentHealth = currentHealth-amount;
        
        if(currentHealth <0)
        {
            currentHealth = 0;
        }

        int howMuchDamage= (int) oldCurrentHealth - (int) currentHealth;

        if(animatedInstance != null){
            animatedInstance.TakeDamage();
        }

        if(damageSource == EntityOwnership.ENEMY)
        {
            BattleManager.inst.howMuchDamageEnemyDidPerTurn[BattleManager.inst.turn] += howMuchDamage;
            EventManager.inst.onEnemyDealDamage.Invoke();
        }
        else
        { 
            BattleManager.inst.howMuchDamagePlayerDidPerTurn[BattleManager.inst.turn] += howMuchDamage;
            EventManager.inst.onPlayerDealDamage.Invoke(); 
        }

        if(currentHealth ==0)
        {
           // EventManager.inst.onEnemyBeastDeath.Invoke();

            Die(damageSource);
        }
        foreach (var item in currentHealthBars)
        {item.onHit.Invoke(); }
        
        BattleManager.inst.CheckIfGameContinues();
       
    }

    public virtual void Die(EntityOwnership damageSource)
    {
        
        if(damageSource != EntityOwnership.ERROR){
            CallDeathEvents(damageSource);
        }
        KO = true;
        animatedInstance.Dissolve();
    }

    void CallDeathEvents(EntityOwnership damageSource)
    {
        if(this.GetType() == typeof(PetBehaviour))
        {
            PetBehaviour p = (PetBehaviour)this;
            if(p.ownership == EntityOwnership.ENEMY)
            {
                if(damageSource == EntityOwnership.ENEMY)
                {EventManager.inst.onEnemyPetKilledByEnemy.Invoke();}
                else if(damageSource == EntityOwnership.PLAYER)
                {EventManager.inst.onEnemyPetKilledByPlayer.Invoke();}
                
            }
            else if(p.ownership == EntityOwnership.PLAYER)
            {
                if(damageSource == EntityOwnership.ENEMY)
                {EventManager.inst.onPlayerPetKilledByEnemy.Invoke();}
                else if(damageSource == EntityOwnership.PLAYER)
                {EventManager.inst.onPlayerPetKilledByPlayer.Invoke();}
            }
        }

        if(this.GetType() == typeof(Beast))
        { 
            EntityOwnership ownership = BattleManager.inst.GetBeastOwnership((Beast)this);
            if(ownership == EntityOwnership.PLAYER)
            {
                if(damageSource == EntityOwnership.ENEMY)
                {EventManager.inst.onPlayerBeastKilledByEnemy.Invoke();}
                else if(damageSource == EntityOwnership.PLAYER)
                {EventManager.inst.onPlayerBeastKilledByPlayer.Invoke();}
            }
            else if(ownership == EntityOwnership.ENEMY)
            {
                if(damageSource == EntityOwnership.PLAYER)
                {EventManager.inst.onEnemyBeastKilledByPlayer.Invoke();}
                else if(damageSource == EntityOwnership.ENEMY)
                {EventManager.inst.onEnemyBeastKilledByEnemy.Invoke();}
            }
        }
    }

    
  
    public void Heal(int amount)
    {
        float HealAmount = Mathf.Min(stats().maxHealth -  currentHealth, amount);
        if(HealAmount != 0){
        if(animatedInstance != null)
        {
            animatedInstance.Heal();
            AudioManager.inst.GetSoundEffect().Play(healSFX);
        }
        }
    
		currentHealth = currentHealth + HealAmount;
       
        foreach (var item in currentHealthBars)
        {item.onHit.Invoke(); }
    }

    public virtual Stats stats()
    {
        Stats s = new Stats();
        Debug.LogAssertion("Base Stat function called");
        return s;
    }
}