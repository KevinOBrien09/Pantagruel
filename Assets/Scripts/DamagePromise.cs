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
                
                if(BattleManager.inst.playerRecord.ContainsKey(i))
                { 
                    if(CardManager.inst.promiseDict[id].turnCastOn == BattleManager.inst.turn)
                    {totalDamage+=BattleManager.inst.playerRecord[BattleManager.inst.turn].GetDamageAfterSpecificPoint(OGargs.castOrder);}
                    else
                    {totalDamage+=BattleManager.inst.playerRecord[i].GetAllDamageDealtThisTurn();}


                }
            //     else{ Debug.Log("Turn " + i + " did not yet occur");
            //    continue; }
              
            }
          
            OGargs.stackBehaviour.UpdateBar((float) totalDamage,(float)meterMax);
         
            if(totalDamage >= meterMax)
            {       
                if(CardManager.inst.handDown)
                {
                   BattleManager.inst.StartCoroutine(Penis());
                   
                    IEnumerator Penis()
                    {
                           RemoveEvent(id);
                        while(CardManager.inst.handDown)
                        {yield return null;}
                     
                        CreateSuccessfullActionStack(OGargs);
                        ExecuteEffects(OGargs);
                        ExecuteFluff();
                
                        if(unStackable)
                        {
                            if(CardManager.inst.promiseList.Contains(this))
                            {CardManager.inst.promiseList.Remove(this);}
                        }
                    }
                   return;
                }
                else
                {
                    RemoveEvent(id);
                    CreateSuccessfullActionStack(OGargs);
                    ExecuteEffects(OGargs);
                    ExecuteFluff();
            
                    if(unStackable)
                    {
                        if(CardManager.inst.promiseList.Contains(this))
                        {CardManager.inst.promiseList.Remove(this);}
                    }
                }
            }
        }
        else{
            Debug.Log("lkdsjfslkdfjlksdf");
        }
      

    }

    public override void ExecuteEffects(EffectArgs OGargs)
    {   
        if(OGargs.isPlayer)
        {
            if(CardFunctions.oneEffectIsViable(desiredEffects,OGargs.isPlayer)){
                foreach (var effect in desiredEffects)
                { 
                    EffectArgs arg = new EffectArgs(OGargs.caster,BattleManager.inst.enemyTarget,OGargs.isPlayer,OGargs.card,OGargs.stackBehaviour,OGargs.castOrder);
                    effect.Use(arg); 
                }
            }
        }
       
    }
}