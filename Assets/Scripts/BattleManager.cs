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
    
    public void StartBattle(BattleType battleType)
    {
        inBattle = true;
        turn = 1;
        turnState = TurnState.Player;

        if(battleType == BattleType.Wild)
        {
            RivalBeastManager.inst.CreateEnemyParty(WildEncounterManager.inst.GetEncounter());
            RivalBeastManager.inst.SwapActiveBeast(RivalBeastManager.inst.currentParty[0]);
        }
        Inventory.inst.EnableItemDragOnAll();
        WorldViewManager.inst.EnterBattle();
        BottomPanel.inst.ChangeState(BottomPanel.inst.cards);
        BottomLeftPanel.inst.SwapToStack();
        MusicManager.inst.EnterBattle();
        CardManager.inst.EnterBattle(PlayerParty.inst.activeBeast);
        RightPanelButtonManager.inst.SwapToBattle();
        for (int i = 0; i < startingMana; i++)
        {   
            ManaManager.inst.IncreaseMaxMana(); 
            EnemyAI.inst.IncreaseMaxMana();
        }

        ManaManager.inst.RegenMana();
        EnemyAI.inst.RegenMana();
        EnemyAI.inst.DrawRandomCard();
        EnemyAI.inst.RebuildCardBacks();
  
        EventManager.inst.onBattleStart.Invoke();

        StartCoroutine(q());
        IEnumerator q()
        {
            BattleTicker.inst.Type("Fight or flight");
            yield return new WaitForSeconds(1);
            BattleTicker.inst.Type("Turn " + turn.ToString());
        }
        
    }

    public bool CheckIfGameContinues()
    {
        if(PlayerParty.inst.activeBeast.currentHealth <= 0)
        {
            if(partyHasValidMember(PlayerParty.inst.party))
            {
                Debug.Log("ForceSwap");
                return true;
            }
            else
            {
                Debug.Log("Lose");
                return false;;
            }
        }
         
        if(RivalBeastManager.inst.activeBeast.currentHealth <= 0)
        {
            if(partyHasValidMember(RivalBeastManager.inst.currentParty))
            {
                Debug.Log("Force Enemy swap");
                return true;}
            else
            {
                Win();
                return false;
            }
        }

      //  Debug.LogAssertion("UH OH!");
        return true;

        
    }

    public void Win(){
        Debug.Log("Win");
        CardManager.inst.DeactivateHand();
        StartCoroutine(q());

        IEnumerator q()
        {
            yield return new WaitForSeconds(1.5f);
            LeaveBattle();
        }
    }

    public bool partyHasValidMember(List<Beast> b)
    {
        List<bool> kos = new List<bool>();
        foreach (var item in b)
        {kos.Add(item.KO);}

        if(!kos.Contains(false))
        {return false;}
        else
        {return true;}
    }

    public void EndTurn()
    {
       if(CheckIfGameContinues()) 
       {
            EventManager.inst.onNewTurn.Invoke();
            Inventory.inst.itemsUsedThisTurn.Clear();
            List<CardStackBehaviour> castOrder = new List<CardStackBehaviour>();
            foreach (Transform item in castOrderHolder)
            {
                CardStackBehaviour action = item.GetComponent<CardStackBehaviour>();
                castOrder.Add(action);
            }
    
            //Do Stuff
            CardManager.inst.CheckForHandFuckery();
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
        
    }

    public void SwapToEnemyTurn()
    {      
        EventManager.inst.onNewEnemyTurn.Invoke();
        CardManager.inst.NewTurn();
        BattleTicker.inst.Type("The beast readies itself.");
        CardManager.inst.DeactivateHand();
       
        EndTurnButton.inst.Deactivate();        
        Inventory.inst.DeactivateDrag();
        EnemyAI.inst.Act(RivalBeastManager.inst.activeBeast);
    }

    public void SwapToPlayerTurn()
    {
        EventManager.inst.onNewPlayerTurn.Invoke();
        CardManager.inst.NewTurn();
        CardManager.inst.ActivateHand();
        Inventory.inst.ActivateDrag();
 
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
        
            CardManager.inst.DrawRandomCard(PlayerParty.inst.activeBeast);
            EventManager.inst.onPlayerDrawingCardFirstTimeInTurn.Invoke();
            CardManager.inst.MakeHandInteractable();
            EndTurnButton.inst.Reactivate();
             yield return new WaitForSeconds(.6f);
            BattleTicker.inst.Type("Make your move.");
        }
    }

    public void LeaveBattle()
    {
        inBattle = false;
        EventManager.inst.onBattleEnd.Invoke();
        turn = 0;
        PlayerManager.inst.movement.ReactivateMove();
        WorldViewManager.inst.LeaveBattle();
        BottomPanel.inst.ChangeState(BottomPanel.inst.dialog);
        BottomLeftPanel.inst.SwapToArrows();
        MusicManager.inst.ChangeToDungeon();
        CardManager.inst.LeaveBattle();
        ManaManager.inst.LeaveBattle();
        EnemyAI.inst.LeaveBattle();
        RivalBeastManager.inst.Wipe();
      
        BattleTicker.inst.Type(LocationManager.inst.currentLocation.locationName);
        Inventory.inst.DisableItemDragOnAll();
        RightPanelButtonManager.inst.SwapToOverworld();
    }

}