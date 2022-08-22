using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastStatusEffectHandler
{
    public Stats modifications = new Stats();
    public Dictionary<string,StatusEffectMonoBehaviour> dict = new Dictionary<string, StatusEffectMonoBehaviour>();
    Beast b;
    
    public void Init(Beast beast)
    {
        b = beast;
    }

    public void ApplyStatusEffect(StatusEffectInstance i,string cause)
    {
        if(b.allience == Alliance.Player)
        {
            if(dict.ContainsKey(cause))
            {
                dict[cause].statusEffectInstances.Add(i);
                dict[cause].UpdateInfoDisplay();
                i.Apply(this);
            }
            else
            {
                StatusEffectMonoBehaviour mb = StatusEffectManager.inst.MakeIcon(i);
                mb.statusEffectInstances.Add(i);
                dict.Add(cause,mb);
                dict[cause].UpdateInfoDisplay();
                i.Apply(this);
            }
        }
        else
        {
            Debug.Log("EnemyStatusEffect");

            if(dict.ContainsKey(cause))
            {
                dict[cause].statusEffectInstances.Add(i);
                dict[cause].UpdateInfoDisplay();
                i.Apply(this);
            }
            else
            {
                StatusEffectMonoBehaviour mb = StatusEffectManager.inst.MakeEnemyIcon(i);
                mb.statusEffectInstances.Add(i);
                dict.Add(cause,mb);
                dict[cause].UpdateInfoDisplay();
                i.Apply(this);
            }


        }
    }

    public void Tick()
    {
        Dictionary<string,StatusEffectInstance> i = new Dictionary<string, StatusEffectInstance>();
        
        foreach (var item in dict)
        {   
            foreach(var b in item.Value.statusEffectInstances)
            {
                if(b.Iterate())
                { i.Add(item.Key,b); }
            }
            item.Value.UpdateInfoDisplay();
        }
        
        foreach (var item in i)
        {
            StatusEffectMonoBehaviour semb =  dict[item.Key];
            if(semb.RemoveInstance(i[item.Key]))
            {dict.Remove(item.Key);}
        }
    }

    public void ModifyStats(StatModification mod)
    {   
        StatModData data = mod.statModData;
    
        switch (data.statName)
        {
            case StatName.Physical:
            modifications.physical += data.change;
            
            break;
            case StatName.Magic:
            modifications.magic += data.change;
            Debug.Log( modifications.magic);
            break;
            case StatName.Toughness:
            modifications.toughness += data.change;
            if(data.change > 0){
            GenericSounds.inst.BuffSFX();
            }else{
            // GenericSounds.inst.DebuffSFX();
            }
           
            break;
            case StatName.Resolve:
            modifications.resolve += data.change;
            break;
            case StatName.Charisma:
            modifications.charisma += data.change;
            break;
        }

        if(b.allience == Alliance.Player){
        BattleManager.inst.beastDisplay.ApplyStats(b.data.stats,modifications);
        }
    }
    
    public void ResetStat(StatModification mod)
    {
        StatModData d = mod.statModData;
        switch (d.statName)
        {
            case StatName.Physical:
            modifications.physical += d.reset;
            break;
            case StatName.Magic:
            modifications.magic += d.reset;
            break;
            case StatName.Toughness:
            modifications.toughness += d.reset;
            break;
            case StatName.Resolve:
            modifications.resolve += d.reset;
            break;
            case StatName.Charisma:
            modifications.charisma += d.reset;
            break;
        }
        
        if(b.allience == Alliance.Player)
        { BattleManager.inst.beastDisplay.ApplyStats(b.data.stats,modifications); }
    }

    public void Bleed(int amount)
    {
        Debug.Log(b.data.beastName);
        b.IgnoreArmour(amount,true);
        b.HitVisual(amount);
    }
    
    

}