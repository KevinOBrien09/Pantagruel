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
                if(BattleManager.inst.howMuchDamagePlayerDidPerTurn.ContainsKey(i))
                { totalDamage+=BattleManager.inst.howMuchDamagePlayerDidPerTurn[i]; }
                else{ Debug.Log("Turn " + i + " did not yet occur"); }
              
            }
            OGargs.stackBehaviour.UpdateBar((float) totalDamage,(float)meterMax);
            Debug.Log(totalDamage + " Dealt by player");
            if(totalDamage >= meterMax)
            {
                CreateSuccessfullActionStack(OGargs);
                ExecuteEffects(OGargs);
                ExecuteFluff();
                RemoveEvent(id);
                if(unStackable)
                {
                    if(CardManager.inst.promiseList.Contains(this))
                    {CardManager.inst.promiseList.Remove(this);}
                }
            
            }
        }
        else{
            Debug.Log("lkdsjfslkdfjlksdf");
        }
      

    }
}