using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Effect :ScriptableObject
{
    public List<EffectTarget> targets = new List<EffectTarget>();
  
    public bool offensive;
   
    public virtual void  Use(EffectArgs args)
    {
        Debug.Log(name);
    }

     public int Percentage(Entity b, StatName s,int percentage)
    {
        return  (int) Maths.Percent(b.stats().GetStat(s),percentage);
  

    }

   
   

    public List<Entity> AffectedEntities(EffectArgs args)
    {
        List<Entity> entities = new List<Entity>();
        foreach (var item in targets)
        {
            Entity e = GetTarget(item,args);
            if(e != null)
            {entities.Add(e);}
        }
       return entities;
    }

    public Entity GetTarget(EffectTarget target,EffectArgs args)
    {
       if(args.caster.OwnedByPlayer())
       {
            switch (target)
            {
                case EffectTarget.CASTER:
                return args.caster;

                case EffectTarget.MYTARGET:
                return BattleManager.inst.playerTarget;
                case EffectTarget.MYBEAST:
                return PlayerParty.inst.activeBeast;
                case EffectTarget.MYPET:
                return PetManager.inst.playerPet;

                case EffectTarget.RIVALTARGET:
                return BattleManager.inst.enemyTarget;
                case EffectTarget.RIVALBEAST:
                return RivalBeastManager.inst.activeBeast;
                case EffectTarget.RIVALPET:
                return PetManager.inst.enemyPet;
                default:
                Debug.Log("DEFAULT CASE.");
                return null;
            }
        }
        else
        {
            switch (target)
            {
                case EffectTarget.CASTER:
                return args.caster;

                case EffectTarget.MYTARGET:
                return BattleManager.inst.enemyTarget;
                case EffectTarget.MYBEAST:
                return RivalBeastManager.inst.activeBeast;
                case EffectTarget.MYPET:
                return PetManager.inst.enemyPet;
                
                case EffectTarget.RIVALTARGET:
                return BattleManager.inst.playerTarget;
                case EffectTarget.RIVALBEAST:
                return PlayerParty.inst.activeBeast;
                case EffectTarget.RIVALPET:
                return PetManager.inst.playerPet;
                default:
                Debug.Log("DEFAULT CASE.");
                return null;
            }

        }
    }


   

    public virtual bool canUse(bool isPlayer){
        return true;
    }
} 
public enum EffectTarget{CASTER,RIVALTARGET,RIVALPET,RIVALBEAST,MYPET,MYBEAST,MYTARGET,UTILITY}

public class EffectArgs
{
    public Entity caster;
    public Card card; 
    public string tickerTitle;
    public CardStackBehaviour stackBehaviour;
    public int castOrder;
    public Entity target;
 
    

    public EffectArgs(Entity CASTER,Entity TARGET,Card CARD,CardStackBehaviour STACK,int CASTORDER,string TICKERTITLE)
    {
        caster = CASTER;
        target = TARGET;
        // target =TARGET;
        // isPlayer = ISPLAYER;
        card = CARD;
        stackBehaviour = STACK;
        castOrder = CASTORDER;
        tickerTitle = TICKERTITLE;
      
        
    }
}

[System.Serializable]
public class AdditionalEffect{
    

}