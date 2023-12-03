
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/DamagePromise", fileName = "DamagePromise")]
public class DamagePromise :Promise
{
    public int howMuchDamage, howManyTurns;

    public override void Use(EffectArgs args)
    {
        string id =  Guid.NewGuid().ToString(); //check if already in dict
        if(args.caster.OwnedByPlayer())
        {
          foo:
            if(!CardManager.inst.promiseDict.ContainsKey(id))
            { SetUpPlayerPromise(id,args); }
            else{
                Debug.LogAssertion("HOLY SHIT IDENTICAL GUID :o");
                id = Guid.NewGuid().ToString(); 
                goto foo;
            }
        }
        else
        {Debug.Log("Add enemy promises");}
        DamageTracker.inst.AddEffect(this,id,args.caster.OwnedByPlayer());
        
    }
    
    public override void PromiseFufilled(EffectArgs OGargs, string id)
    {
        int damageDealtSinceCast = DamageTracker.inst.GetDamage(id);
        bool success =  damageDealtSinceCast  >= meterMax;
        OGargs.stackBehaviour.UpdateBar((float)damageDealtSinceCast,(float)meterMax);

        if(success)
        {
            //base.PromiseFufilled(OGargs, id);
            DamageTracker.inst.RemoveEffect(id);


            BattleManager.QueuedAction qa = new BattleManager.QueuedAction();
            UnityAction a = ()=> poopoo();
            qa.action = a;
            qa.args = OGargs;
            BattleManager.inst.effectsToUse.Enqueue(qa);
            RemoveEvent(id);
            void poopoo()
            {
                ExecuteEffects(OGargs);
                CreateSuccessfullActionStack(OGargs);
                
                ExecuteFluff(OGargs);
        
                if(unStackable)
                {
                    if(CardManager.inst.promiseList.Contains(this))
                    {CardManager.inst.promiseList.Remove(this);}
                }
            }
           
        }
        else
        {
            Debug.Log("Not Yet.");
        }

      
    }

    public override void ExecuteBadEffects(EffectArgs OGargs,string id = "")
    {
        DamageTracker.inst.RemoveEffect(id);
        base.ExecuteBadEffects(OGargs);
        Debug.Log("BADSTUFF XC");
    }
}