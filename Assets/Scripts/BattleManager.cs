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
        {   
            ManaManager.inst.IncreaseMaxMana(); 
            EnemyAI.inst.IncreaseMaxMana();
        }
        ManaManager.inst.RegenMana();
        EnemyAI.inst.RegenMana();
        EnemyAI.inst.DrawCard();
        EnemyAI.inst.RebuildCardBacks();

        StartCoroutine(q());
        IEnumerator q()
        {
            BattleTicker.inst.Type("Fight or flight");
            yield return new WaitForSeconds(1);
            BattleTicker.inst.Type("Turn " + turn.ToString());
        }
        
    }
    public void CheckIfGameContinues()
    {
        if(PlayerManager.inst.party.activeBeast.currentHealth <=0){
            Debug.Log("Force Player to swap");
        }
         
         if(RivalBeastManager.inst.currentBeast.currentHealth <=0){
            Debug.Log("win");
         }
    }

    public void EndTurn()
    {
        CheckIfGameContinues();
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
        BattleTicker.inst.Type("The beast readies itself.");
        CardManager.inst.DeactivateHand();
        EndTurnButton.inst.Deactivate();        
        EnemyAI.inst.Act(RivalBeastManager.inst.currentBeast);
    }

    public void SwapToPlayerTurn()
    {
       
        CardManager.inst.ActivateHand();
        StartCoroutine(q());
        
        if(turn % 2 == 0)
        {
            ManaManager.inst.IncreaseMaxMana();
            EnemyAI.inst.IncreaseMaxMana();
        }
        ManaManager.inst.RegenMana();
        EnemyAI.inst.RegenMana();
        turn++;
        BattleTicker.inst.Type("Turn " + turn.ToString());
        IEnumerator q()
        {
            yield return new WaitForSeconds(.25f);

            CardManager.inst.DrawRandomCard(PlayerManager.inst.party.activeBeast);

            CardManager.inst.MakeHandInteractable();
            EndTurnButton.inst.Reactivate();
             yield return new WaitForSeconds(.6f);
            BattleTicker.inst.Type("Make your move.");
        }
    }

}