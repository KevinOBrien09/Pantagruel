using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
[System.Serializable]
[CreateAssetMenu(menuName = "Effects/DamagePromise", fileName = "DamagePromise")]
public class DamagePromise :Promise
{
    public override void PromiseFufilled(EffectArgs OGargs,   string id)
    {
        if(OGargs.isPlayer)
        {
            int totalDamage = 0;
            for (int i = CardManager.inst.promiseDict[id].turnCastOn; i < CardManager.inst.promiseDict[id].turnToDieOn; i++)
            {
                int forLoopTurn = i;
                if(BattleManager.inst.playerRecord.ContainsKey(forLoopTurn))//bug prone
                { 
                    if(CardManager.inst.promiseDict[id].turnCastOn == BattleManager.inst.turn)
                    {totalDamage+=BattleManager.inst.playerRecord[BattleManager.inst.turn].GetDamageAfterSpecificPoint(OGargs.castOrder);}
                    else if(forLoopTurn != CardManager.inst.promiseDict[id].turnCastOn)
                    {totalDamage+=BattleManager.inst.playerRecord[forLoopTurn].GetAllDamageDealtThisTurn();} 
                   
                }
            }
            
            OGargs.stackBehaviour.UpdateBar((float) totalDamage,(float)meterMax);
         
            if(totalDamage >= meterMax)
            {       
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
        }
    }

    public override void ExecuteEffects(EffectArgs OGargs)
    {   
        if(OGargs.isPlayer)
        {
            if(CardFunctions.oneEffectIsViable(desiredEffects,OGargs.isPlayer)){
                foreach (var effect in desiredEffects)
                { 
                    EffectArgs arg = new EffectArgs(OGargs.caster,BattleManager.inst.enemyTarget,OGargs.isPlayer,OGargs.card,OGargs.stackBehaviour,OGargs.castOrder,OGargs.card.cardName);
                    effect.Use(arg); 
                }
            }
        }
       
    }
}