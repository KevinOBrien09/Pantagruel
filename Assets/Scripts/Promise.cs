using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Promise", fileName = "Promise")]
public class Promise :Effect
{
    public List<Effect> desiredEffects,badEffects;
    public Card parentCard;
    public string vfx;
    public SoundData soundData;
    public List<EventEnum> eventEnums = new List<EventEnum>();
    public int turnDuration;

    public override void Use(EffectArgs args)
    {
        if(args.isPlayer)
        {
            UnityAction u = () =>Act();
            foreach (var item in eventEnums)
            {EventManager.inst.AssignEvent(item,u);}
            CardManager.inst.promiseDict.Add(this, u);
            
        }
    }

    public virtual void Act()
    {
        foreach (var effect in desiredEffects)
        { 
            EffectArgs args = new EffectArgs(PlayerParty.inst.activeBeast,BattleManager.inst.enemyTarget,true,parentCard);
            effect.Use(args); 
        }
        AudioManager.inst.GetSoundEffect().Play(soundData);
        BattleEffectManager.inst.Play(vfx);

       RemoveEvent();
        CardManager.inst.promiseDict.Remove(this);
    }

    public void RemoveEvent(){
        foreach (var item in eventEnums)
        {EventManager.inst.RemoveEvent(item,CardManager.inst.promiseDict[this]);}
    }

    public virtual bool CheckPromise()
    {
        return false;
    }

}