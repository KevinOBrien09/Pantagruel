using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnPasser:Singleton<TurnPasser>
{
    public void Pass()
    {
        if(BattleManager.inst.currentBattleState == BattleManager.BattleState.PlayerTurn){
            BattleManager.inst.StartCoroutine(  BattleManager.inst.PassTurn());
        }
        
    }


}