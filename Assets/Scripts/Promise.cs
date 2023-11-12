using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Promise", fileName = "Promise")]
public class Promise :Effect
{
    public List<Effect> desiredEffects,badEffects;
    public Card parentCard;
    public string vfx;
    public SoundData soundData;
    public List<EventEnum> eventEnums = new List<EventEnum>();
    public bool unStackable;
    public int turnDuration;
    public bool turnLimit;
    public bool meter;
     public int meterMax;

    public override void Use(EffectArgs args)
    {  
        string id =  Guid.NewGuid().ToString(); //check if already in dict
        if(args.isPlayer)
        {
          foo:
            if(!CardManager.inst.promiseDict.ContainsKey(id))
            {
                UnityAction u = ()=> PromiseFufilled(args,id);
                ActionEventPair pair = new ActionEventPair();
                pair.ID = id;
                pair.action = u;
                pair.args = args;
                pair.turnCastOn = BattleManager.inst.turn;
                if(turnLimit)
                {
                   int ttdo = BattleManager.inst.turn + turnDuration + 1;//counts same turn add 1 to negate
                   pair.turnToDieOn = ttdo;
                }
                foreach (var item in eventEnums)
                {
                    EventManager.inst.AssignEvent(item,u);
                    pair.subbedEvents.Add(item);
                }
                CardManager.inst.promiseDict.Add(id,pair);

                if(unStackable){
                CardManager.inst.promiseList.Add(this);
                }
               
            }
            else{
                Debug.LogAssertion("HOLY SHIT IDENTICAL GUID :o");
                id = Guid.NewGuid().ToString(); 
                goto foo;
            }
        }
    }

    public override bool canUse(bool isPlayer)
    {
        if(!unStackable){
            return true;
        }
        else{
            if(CardManager.inst.promiseList.Contains(this)){
                return false;
            }
            else{
                return true;
            }
        }
        
    }


    public virtual void PromiseFufilled(EffectArgs OGargs,   string id)
    {
        
        ExecuteEffects(OGargs);
        CreateSuccessfullActionStack(OGargs);
        ExecuteFluff();
        RemoveEvent(id);
        if(unStackable)
        {
            if(CardManager.inst.promiseList.Contains(this))
            {CardManager.inst.promiseList.Remove(this);}
        }
    }

    public void ExecuteFluff(){
        AudioManager.inst.GetSoundEffect().Play(soundData);
        BattleEffectManager.inst.Play(vfx);
    }

    public void ExecuteEffects(EffectArgs OGargs)
    {
        if(CardFunctions.oneEffectIsViable(desiredEffects,OGargs.isPlayer)){
            foreach (var effect in desiredEffects)
            { 
                EffectArgs arg = new EffectArgs(OGargs.caster,OGargs.target,OGargs.isPlayer,parentCard,null);
                effect.Use(arg); 
            }
        }
       
    }

   public void CreateSuccessfullActionStack(EffectArgs OGargs){
  CardStackBehaviour b = CardStack.inst.CreateActionStack( OGargs.card,(Beast) OGargs.caster,OGargs.isPlayer);
        b.ConditionFufilled();
    }

    public void RemoveEvent(string id)
    {
        foreach (var item in eventEnums)
        {EventManager.inst.RemoveEvent(item,CardManager.inst.promiseDict[id].action);}
         CardManager.inst.promiseDict.Remove(id);
    }

    public virtual bool CheckPromise()
    {
        return false;
    }

}