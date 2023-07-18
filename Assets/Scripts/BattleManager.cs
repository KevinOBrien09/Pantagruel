 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleManager:Singleton<BattleManager>                   
{
    public enum TurnState{Player,Enemy}
    public TurnState turnState;
    public bool inBattle;
    public RectTransform castOrderHolder;
    public int turn;
    public int startingMana;
    
    public void StartBattle()
    {
        inBattle = true;
        turn = 1;
        turnState = TurnState.Player;

        RivalBeastManager.inst.EnterBattle();
        WorldViewManager.inst.EnterBattle();
        BottomPanel.inst.ChangeState(BottomPanel.inst.cards);
        BottomLeftPanel.inst.SwapToStack();
        MusicManager.inst.EnterBattle();
        CardManager.inst.EnterBattle(PlayerManager.inst.party.activeBeast);
        
        for (int i = 0; i < startingMana; i++)
        { ManaManager.inst.IncreaseMaxMana(); }
        ManaManager.inst.RegenMana();
    }

    public void EndTurn()
    {
        List<CardStackBehaviour> castOrder = new List<CardStackBehaviour>();
        foreach (Transform item in castOrderHolder)
        {
            CardStackBehaviour action = item.GetComponent<CardStackBehaviour>();
            castOrder.Add(action);
        }

        //Do Stuff
       
        foreach (var item in castOrder)
        {
            Destroy(item.gameObject);
        }

        if(turnState == TurnState.Player)
        {
            turnState = TurnState.Enemy;
            SwapToEnemyTurn();
        }
        else if(turnState == TurnState.Enemy)
        {
            turnState = TurnState.Player;
            SwapToPlayerTurn();
        }
    }

    public void SwapToEnemyTurn()
    {
        CardManager.inst.DeactivateHand();
        EndTurnButton.inst.Deactivate();        
        EnemyAI.inst.Act();
    }

    public void SwapToPlayerTurn()
    {
        CardManager.inst.ActivateHand();
        StartCoroutine(q());
        
        if(turn % 2 == 0)
        {ManaManager.inst.IncreaseMaxMana();}
        ManaManager.inst.RegenMana();
        turn++;

        IEnumerator q()
        {
            yield return new WaitForSeconds(.25f);

            CardManager.inst.DrawRandomCard(PlayerManager.inst.party.activeBeast);

            CardManager.inst.MakeHandInteractable();
            EndTurnButton.inst.Reactivate();
        }
    }

}