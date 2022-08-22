using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Beast : MonoBehaviour
{
    public delegate void BeastEvent(int dmg);
    public event BeastEvent onHit;
    public event BeastEvent on_SP_Drain;
    public event BeastEvent onHeal;
    public BeastMovementHandler movementHandler = new BeastMovementHandler();
    public BeastStatusEffectHandler statusEffectHandler = new BeastStatusEffectHandler();
    [SerializeField] BeastScriptableObject DEBUGOBJECT;
    [SerializeField] GameObject PLACEHOLDERPREFAB;
    public BeastData data;
    public Alliance allience;
    public int currentHealth;
    public int currentSP;
    public  HealthBar healthBar;
    [SerializeField] BeastGraphic graphic;
    [SerializeField] List<SoundData> sfx = new List<SoundData>();
    [SerializeField] List<SoundData> bleedSound = new List<SoundData>();
    [SerializeField] ParticleSystem bleed;
  //  public List<StatusEffect> statusEffects = new List<StatusEffect>();
    
    void Awake()
    {
        onHit += DeduceHealth;
        on_SP_Drain += DeduceSP;
        onHeal += HealHealth;
    }

    void Start()
    {
        if(allience == Alliance.Player)
        {
            data = new BeastData(DEBUGOBJECT);
            Init(DEBUGOBJECT,Alliance.Player);
            BattleManager.inst.ApplyPlayerBeastInfo(this);
        }
    }

    public void Init(BeastScriptableObject bso,Alliance a)
    {
        data = new BeastData(bso);
        gameObject.name = data.beastName;
        currentHealth = data.stats.maxHealth;

        if(data.resource.resource == ResourceCurrency.SP)
        {currentSP = data.resource.amount;}  

        GameObject go = null;
        if(data.ANIMATED_PREFAB_DONE)
        {
            go = Instantiate(data.beastGraphicPrefab,transform);
            graphic = go.GetComponent<BeastGraphic>();
            onHit += HitVisual;
            onHit += sound;
        }
        else
        {
            go = Instantiate(PLACEHOLDERPREFAB,transform);
            graphic = go.GetComponent<BeastGraphic>();
            graphic.sprites[0].sprite = data.mainSprite;
            onHit += HitVisual;
            onHit += sound;
            Debug.LogWarning("Placeholder");
        }
        
        if(a == Alliance.Player)
        {
            if(data.ANIMATED_PREFAB_DONE)
            {
                foreach (Transform item in go.transform)
                {item.gameObject.layer = 8;}
                bleed.gameObject.layer = 8;
            }
            else
            {
                graphic.sprites[0].sprite = data.mainSprite;
                graphic.sprites[0].gameObject.layer = 8;
                  bleed.gameObject.layer = 8;
            }
            onHit += DamageEffect;
        }
        
        movementHandler.Init(graphic,transform,transform.position);
        statusEffectHandler.Init(this);
    }

    public void HitVisual(int i)
    { 
        if( i > 0)
        {graphic.StartCoroutine(graphic.HitCoro(true));}
    }

    void sound(int i)
    {StartCoroutine(HitSoundDelay());}
    
    IEnumerator HitSoundDelay()
    {
        yield return new WaitForSeconds(.1f);
        AudioManager.inst.GetSoundEffect().Play(sfx[Random.Range(0,sfx.Count)]);
    }
    
    public void MakeActive()
    {graphic.ChangeLayersToForefront();}

    public void DamageEffect(int dmg)
    {
        if( dmg > 0)
        {BattleManager.inst.splash.Shake();}
    }
    
    public void DeduceHealth(int dmg)
    {
        float armour = GetModifiedStat(StatName.Toughness);
      
        armour = Mathf.Clamp(armour,-90,90); 
        float damageReducion = (float)Maths.Percent(dmg,(int)armour);
        float d = dmg - damageReducion;
        float c = (float)currentHealth -(float)d;
        currentHealth = Mathf.FloorToInt(c);
        currentHealth = (int)Mathf.Clamp((float)currentHealth, (float)0, (float)data.stats.maxHealth);

        if(currentHealth <= 0){
            graphic.Fade();
        }
    }

    public void IgnoreArmour(int cost,bool playSound){

        float c = (float)currentHealth -(float)cost;
        currentHealth = Mathf.FloorToInt(c);
        currentHealth = (int)Mathf.Clamp((float)currentHealth, (float)0, (float)data.stats.maxHealth);
        if(playSound){
            AudioManager.inst.GetSoundEffect().Play(bleedSound[Random.Range(0,bleedSound.Count)]);
            if(allience == Alliance.Player){
            BattleManager.inst.splash.SoloBlood();
            }
           
            bleed.Play();
        }
        BattleManager.inst.BattleMusic();
        healthBar.UpdateHealthBar();
        if(currentHealth <= 0){
            graphic.Fade();
        }
    }

    public int GetModifiedStat(StatName stat)
    {
        switch (stat)
        {
            case StatName.Physical:
            return data.stats.physical +  statusEffectHandler.modifications.physical;
            case StatName.Magic:
            return data.stats.magic +  statusEffectHandler.modifications.magic;
            case StatName.Toughness:
            return data.stats.toughness +  statusEffectHandler.modifications.toughness;
            case StatName.Charisma:
            return data.stats.charisma +  statusEffectHandler.modifications.charisma;
            case StatName.Resolve:
            return data.stats.resolve +  statusEffectHandler.modifications.resolve;
            default:
            Debug.LogAssertion("Stat Not Found, Default Case");
            return 0;
        }

    }
    
    public void OnHit(int dmg)
    {   
        if(onHit!=null)
        {onHit(dmg);}
        BattleManager.inst.BattleMusic();
    }
    
    public void HealHealth(int heal)
    {
        int healAmount = Mathf.Min(data.stats.maxHealth -  currentHealth, heal);
		currentHealth = currentHealth + healAmount;
    }
    
    public void OnHeal(int heal)
    {
        if(onHeal != null)
        {onHeal(heal);}
        GenericSounds.inst.HealSFX();
    }
    
    public void DeduceSP(int sp)
    {
        float c = (float)currentSP -(float) sp;
        currentSP = Mathf.FloorToInt(c);
        currentSP = (int)Mathf.Clamp((float)currentSP, (float)0, (float)data.resource.amount);
    }
    
    public void On_SP_Drain(int sp)
    {   
        if(onHit!=null)
        {on_SP_Drain(sp);}
    }
}
