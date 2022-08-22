using UnityEngine;
using System.Collections.Generic;


//Bug when stacking, when stacked twice second stack will not iterate.

[System.Serializable]
public class StatusEffectInstance
{
    public Sprite picture;
    public int howManyTurns;
    public bool noTurnLimit;
    public List<StatusEffect> effects = new List<StatusEffect>();

    public virtual bool Iterate()
    {
     
        Tick();

        if(noTurnLimit)
        {return false;}

        howManyTurns--;

        if(howManyTurns <= 0)
        { return true; }
        else{ return false; }
    }

    public void Apply(BeastStatusEffectHandler eH)
    {
        foreach (var item in effects)
        { item.Apply(eH); }
    }

    public void Tick()
    {
        foreach (var item in effects)
        { item.Tick(); }
    }

    public void Exit()
    {
        foreach (var item in effects)
        { item.Exit(); }
    }
    
    protected virtual void DoThing()
    {
      
    }
}

public enum StatusEffectType{StatMod,Bleed}
[System.Serializable]
public class StatusEffectData
{
    public StatusEffectType statusEffectType;
    public int howManyTurns;
    public BleedData bleedData;
    public StatModData statModData;
}

[System.Serializable]
public struct BleedData
{
    public int damagePerTurn;
}

[System.Serializable]
public struct StatModData
{
    public StatName statName;
    public int change;
    public int reset
    {
        get
        { 
            if(change > 0)
            { return -change; }
            else
            { return Mathf.Abs(change); }
          
        }
    }
}

public class StatusEffect
{
    public BeastStatusEffectHandler effectHandler;
    public virtual void Apply(BeastStatusEffectHandler eH)
    {
        effectHandler = eH;

    }

    public virtual void Tick()
    {
       
    }

    public virtual void Exit()
    {

    }
}

public class StatModification:StatusEffect
{
    public StatModData statModData;

    public override void Apply(BeastStatusEffectHandler eH)
    {
        base.Apply(eH);
        effectHandler.ModifyStats(this);
    }

    public override void Exit()
    {
        base.Exit();
        effectHandler.ResetStat(this);
    }

    public StatModification(StatModData data)
    {statModData = data;}
}

public class Bleed:StatusEffect
{
   public BleedData bleedData;
    
    
    public override void Apply(BeastStatusEffectHandler eH)
    {
        base.Apply(eH);
      
    }

    public override void Tick()
    {
        effectHandler.Bleed(bleedData.damagePerTurn);
    }

    public Bleed(BleedData data){
       bleedData = data;
    }
}






