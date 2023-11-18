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
    public StatusEffectHandler statusEffectHandler;
    
    public void TakeDamage(float amount,EffectArgs args)
    {
        float oldCurrentHealth = currentHealth;
        currentHealth = currentHealth-amount;
        EntityOwnership damageSource = EntityOwnership.ERROR;
        if(args.isPlayer){
            damageSource = EntityOwnership.PLAYER;
        }
        else{
            damageSource = EntityOwnership.ENEMY;
        }
        
        if(currentHealth <0)
        {
            currentHealth = 0;
        }

        int howMuchDamage= (int) oldCurrentHealth - (int) currentHealth;

        if(animatedInstance != null){
            animatedInstance.TakeDamage();
        }

        
        if(currentHealth ==0)
        {
           // EventManager.inst.onEnemyBeastDeath.Invoke();

            Die(damageSource);
        }

        if(damageSource == EntityOwnership.ENEMY)
        {
            BattleManager.TurnRecord.CardIntPair p = new BattleManager.TurnRecord.CardIntPair();
            p.card = args.card;
            p.v = howMuchDamage;
            p.castOrder = args.castOrder;
            BattleManager.inst.enemyRecord[BattleManager.inst.turn].damageDealtByEachCard.Add(p);
            EventManager.inst.onEnemyDealDamage.Invoke();
        }
        else
        { 
            BattleManager.TurnRecord.CardIntPair p = new BattleManager.TurnRecord.CardIntPair();
            p.card = args.card;
            p.v = howMuchDamage;
            p.castOrder = args.castOrder;
            BattleManager.inst.playerRecord[BattleManager.inst.turn].damageDealtByEachCard.Add(p);
            EventManager.inst.onPlayerDealDamage.Invoke(); 
        }

    
        foreach (var item in currentHealthBars)
        {item.onHit.Invoke(); }
        
        BattleManager.inst.CheckIfGameContinues();
       
    }

    public void Bleed(EffectArgs args)
    {
        float oldCurrentHealth = currentHealth;
        currentHealth = currentHealth - Mathf.RoundToInt( (float) Maths.Percent(stats().maxHealth,6)) ;
        int howMuchDamage= (int) oldCurrentHealth - (int) currentHealth;
        Debug.Log("BLEED " + howMuchDamage);
        EntityOwnership damageSource = EntityOwnership.ERROR;
        if(args.isPlayer)
        {damageSource = EntityOwnership.PLAYER;} 
        else{damageSource = EntityOwnership.ENEMY;}

        if(animatedInstance != null){
            animatedInstance.TakeDamage();
        }
        if(currentHealth <0)
        {
            currentHealth = 0;
        }

        if(currentHealth ==0)
        {
           

            Die(damageSource);
        }

        if(damageSource == EntityOwnership.ENEMY)
        {
            BattleManager.inst.enemyRecord[BattleManager.inst.turn].bleedDamage += howMuchDamage;
            EventManager.inst.onEnemyDealDamage.Invoke();
        }
        else
        { 
            BattleManager.inst.playerRecord[BattleManager.inst.turn].bleedDamage += howMuchDamage;
            EventManager.inst.onPlayerDealDamage.Invoke(); 
        }
        foreach (var item in currentHealthBars)
        {item.onHit.Invoke(); }
        
        BattleManager.inst.CheckIfGameContinues();

    }

    public virtual void Die(EntityOwnership damageSource)
    {
        
        if(damageSource != EntityOwnership.ERROR)
        {CallDeathEvents(damageSource);}
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