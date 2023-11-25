 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{    
    public enum GuardState{Open,Guard,GuardBroke}
    public List< HealthBar> currentHealthBars = new List<HealthBar>();
    public BeastAnimatedInstance animatedInstance;
    public SoundData healSFX,shieldSFX;
    public float currentHealth;
    public float currentShield;
    public bool KO;
    public StatusEffectHandler statusEffectHandler;
    public bool goLeft =true;
    //public GuardState guard;
    public EntityOwnership ownership;
    public List<Shield> shields = new List<Shield>();

    void Update(){
        currentShield = totalShield();
    }
   
    
    public void TakeDamage(float amount,EffectArgs args)
    {
        float oldCurrentHealth = currentHealth;
        float paradmg = ParasiteDamage();
        float damage = amount; 
        damage += paradmg;
        float postMitigationDamage =   ShieldHandler(damage);  //may be buggy idk
        currentHealth = currentHealth-postMitigationDamage;
        EntityOwnership damageSource = EntityOwnership.ERROR;
        if(args.isPlayer)
        {damageSource = EntityOwnership.PLAYER;}
        else
        {damageSource = EntityOwnership.ENEMY;}
        
        if(currentHealth <0)
        {currentHealth = 0;}
        
 
        int howMuchDamage= (int) oldCurrentHealth - (int) currentHealth;
        SpawnVisualDamage(howMuchDamage -paradmg,Color.white);

        if(animatedInstance != null)
        {animatedInstance.TakeDamage(Color.red);}
        
        if(currentHealth ==0)
        {Die(damageSource);}

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

 
        
        ClearShields();
        foreach (var item in currentHealthBars)
        {item.onHit.Invoke(); }
        
        BattleManager.inst.CheckIfGameContinues();
       
    }

    public float ShieldHandler(float damage)
    {
        if(totalShield() > 0)
        {
            float oldShield = totalShield();
            float leftOverDamage = DeductShield(damage);
            float shieldDamage =  oldShield - totalShield();
      
            return leftOverDamage;
        }
       

        return damage;
    }

    float DeductShield(float dmg)
    {
        if(shields.Count > 0) 
        { 
            Shield currentShield = null;
            foreach (var item in shields)
            {
                if(item.value > 0){
                    currentShield = item;
                    break;
                }
            }

            if(currentShield == null){
                Debug.Log(Mathf.Abs(dmg) + " excess");
                return Mathf.Abs(dmg);
            }
           
            float  p = 0;
            currentShield.value -= dmg;
            if(currentShield.value <0)
            {
             //   ClearShields();
                return  DeductShield( Mathf.Abs(currentShield.value));
            }
            else{
            Debug.Log(p + " excess");
            return p;
            }

       
            
        }
        return 0;
    }

    public void AddShield(StatusEffectDisplay display)
    {
        Shield s = new Shield();
        s.display = display;
        s.value = Mathf.RoundToInt( (float) Maths.Percent(stats().maxHealth,10));
        shields.Add(s);
       //currentShield+= 
       
        foreach (var item in     currentHealthBars)
        {
            item.UpdateFill();
            item.UpdateText();
        }
        AudioManager.inst.GetSoundEffect().Play(shieldSFX);
    
    }

  

    void ClearShields(){
        List<Shield> bin = new List<Shield>();
        foreach (var item in shields)
        {
            if(item.value <= 0){
         bin.Add(item);
        }
        }

        foreach (var item in bin)
        {
            shields.Remove(item);
        }
    }

   public float totalShield(){
        float q = 0;
        foreach (var item in shields)
        {
            q+=item.value;
        }

        return q;
    }


    public void Bleed(bool isPlayer)
    {
        float oldCurrentHealth = currentHealth;
        int damage = Mathf.RoundToInt( (float) Maths.Percent(stats().maxHealth,5)) ;
        damage += ParasiteDamage();
        currentHealth = currentHealth - damage;
        int howMuchDamage= (int) oldCurrentHealth - (int) currentHealth;
         Color c = new Color();
            ColorUtility.TryParseHtmlString("#7100FF",out c);
        SpawnVisualDamage(damage,c);
        Debug.Log("BLEED " + howMuchDamage);
        EntityOwnership damageSource = EntityOwnership.ERROR;
        if(isPlayer)
        {damageSource = EntityOwnership.PLAYER;} 
        else{damageSource = EntityOwnership.ENEMY;}
       // animatedInstance.Bleed();
        AudioManager.inst.GetSoundEffect().Play(SystemSFX.inst.bleed);
        if(animatedInstance != null)
        {
           
            animatedInstance.TakeDamage(c);
        }
        if(currentHealth <0)
        {currentHealth = 0;}
        if(currentHealth ==0)
        {Die(damageSource); }

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

    public int ParasiteDamage()
    {
        int q = 0;
        if(statusEffectHandler!=null){
    foreach (var item in statusEffectHandler.currentEffects)
        {
            if(item == StatusEffects.PARASITE){
                q++;
            }
        }
        if(q!= 0){
       Color c = new Color();
        ColorUtility.TryParseHtmlString("#FF00BD",out c);

       SpawnVisualDamage(q,c);
       }
        }
    
      return q;
    }


    

    public void SpawnVisualDamage(float q, Color c)
    {
        if(goLeft){
            goLeft = false;
        }
        else{
            goLeft = true;
        }
        Debug.Log(gameObject.name);
      DamageValueGraphic d =  Instantiate(BattleManager.inst.damageValueGraphicPrefab, animatedInstance. transform);
      d.gameObject.layer = animatedInstance.gameObject.layer;
      if(statusEffectHandler!=null){  d.transform.localPosition = statusEffectHandler  .transform.localPosition;}
      else{Debug.Log("Cannot find handler");}
    
      d.Spawn((int)q,c,goLeft);
        
    }

    public bool OwnedByPlayer(){
        return ownership == EntityOwnership.PLAYER;
    }

   

    public virtual void Die(EntityOwnership damageSource)
    {
        if(statusEffectHandler!=null){
            statusEffectHandler.gameObject.SetActive(false);
        }
        
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
           
            if(OwnedByPlayer())
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