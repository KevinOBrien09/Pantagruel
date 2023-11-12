 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public enum EntityOwnership{ERROR,PLAYER,ENEMY}
public class BattleManager:Singleton<BattleManager>                   
{
    public enum TurnState{Player,Enemy}
    public TurnState turnState;
    public bool inBattle;
    public RectTransform castOrderHolder;
    public int turn;
    public int startingMana;
    public Entity enemyTarget;
    public SoundData leave;
    public Entity playerTarget;
    public StatusEffectManager playerStatusEffects;
    public Dictionary<int,int> howMuchDamagePlayerDidPerTurn = new Dictionary<int, int>();
    public Dictionary<int,int> howMuchDamageEnemyDidPerTurn = new Dictionary<int, int>();
    
    public void StartBattle(BattleType battleType)
    {
        inBattle = true;
        turn = 1;
           howMuchDamageEnemyDidPerTurn.Add(turn,0);
        howMuchDamagePlayerDidPerTurn.Add(turn,0);
        turnState = TurnState.Player;

        if(battleType == BattleType.Wild)
        {
            RivalBeastManager.inst.CreateEnemyParty(WildEncounterManager.inst.GetEncounter());
            RivalBeastManager.inst.SwapActiveBeast(RivalBeastManager.inst.currentParty[0]);
        }
        EndTurnButton.inst.Reactivate();
        BattleField.inst.SetPlayerBeastIcon(PlayerParty.inst.activeBeast);
        BattleField.inst.Init();
        UpperLeftPanel.inst.  SwapToBattle();
        SetEnemyTarget(RivalBeastManager.inst.activeBeast);
        SetPlayerTarget(PlayerParty.inst.activeBeast);
        Inventory.inst.EnableItemDragOnAll();
        WorldViewManager.inst.EnterBattle();
        BottomPanel.inst.ChangeState(BottomPanel.inst.cards);
        LeftPanel.inst.SwapToBattle();
        BottomLeftPanel.inst.SwapToBattle();
        BottomCornerBeastDisplayer.inst.ToggleBattleBGOff();
        MusicManager.inst.EnterBattle();
        CardManager.inst.EnterBattle(PlayerParty.inst.activeBeast);
        RightPanelButtonManager.inst.SwapToBattle();
        for (int i = 0; i < startingMana; i++)
        {   
            ManaManager.inst.IncreaseMaxMana(); 
            EnemyAI.inst.IncreaseMaxMana();
        }
        CardStack.inst.EnterBattle();
        ManaManager.inst.RegenMana();
        EnemyAI.inst.RegenMana();
        EnemyAI.inst.DrawRandomCard();
        EnemyAI.inst.DrawRandomCard();
        EnemyAI.inst.DrawRandomCard();
       // EnemyAI.inst.RebuildCardBacks();
  
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
        if(!inBattle){
            return false;
        }
        bool playerActiveBeastIsDead = PlayerParty.inst.activeBeast.currentHealth <= 0;
        bool enemyActiveDeastIsDead = RivalBeastManager.inst.activeBeast.currentHealth <= 0;
        //bool enemySummonIsDead = 
       
           
        
       

        if(playerActiveBeastIsDead)
        {
          
            if(partyHasValidMember(PlayerParty.inst.party))
            {
                Debug.Log("ForceSwap");
                
                return true;
            }
            else
            {
                Debug.Log("Lose");
                inBattle = false;
                return false;;
            }
        }

        // if(enemyTarget.currentHealth <= 0)
        // {
            if(enemyActiveDeastIsDead)
            {  
                if(partyHasValidMember(RivalBeastManager.inst.currentParty))
                {
                    Debug.Log("Force Enemy swap");
                    return true;}
                else
                {
                    Win();
                    inBattle = false;
                    return false;
                }
            }
            else{
                //move forward set enemybeast target
            }
       // }

      //  Debug.LogAssertion("UH OH!");
        return true;

        
    }

    public void SetEnemyTarget(Entity e){
        enemyTarget = e;
    }

    public void SetPlayerTarget(Entity e){
        playerTarget = e;
    }

    public void Win()
    {
        Debug.Log("Win");
        CardManager.inst.DeactivateHand();
        StartCoroutine(q());

        IEnumerator q()
        {
            EndTurnButton.inst.Deactivate();
            CardManager.inst.DeactivateHand();
           
          
           
            foreach (var item in CardManager.inst.hand)
            {item.VaporousDissolve();}
            
          
            inBattle = false;

            yield return new WaitForEndOfFrame();
              if(PetManager.inst.enemyPet!= null && !PetManager.inst.enemyPet.KO){
               
                PetManager.inst.enemyPet.Die(EntityOwnership.ERROR);
            }
            yield return new WaitForSeconds(1.5f);
            RewardManager.inst.Open();
           // LeaveBattle();
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
         
    
            //Do Stuff
            CardManager.inst.CheckForHandFuckery();
          

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
        IncrementTurn();
        IEnumerator q()
        {
            yield return new WaitForSeconds(.25f);
        
            CardManager.inst.DrawCard();
            EventManager.inst.onPlayerDrawingCardFirstTimeInTurn.Invoke();
            CardManager.inst.MakeHandInteractable();
            EndTurnButton.inst.Reactivate();
             yield return new WaitForSeconds(.6f);
            BattleTicker.inst.Type("Make your move.");
        }
    }

    void IncrementTurn(){
        turn++;
        howMuchDamageEnemyDidPerTurn.Add(turn,0);
        howMuchDamagePlayerDidPerTurn.Add(turn,0);
          CardStack.inst.NewTurn();
        CheckBullshit();
      
        BattleTicker.inst.Type("Turn " + turn.ToString());
    }

    void CheckBullshit()
    {

        foreach (var item in CardManager.inst.promiseDict)
        {
            if(item.Value.turnToDieOn == turn)
            {
                Debug.Log("Remove " + item.Value);
                foreach (var e in item.Value.subbedEvents)
                {EventManager.inst.RemoveEvent(e,item.Value.action);}
                CardStackBehaviour b =  CardStack.inst.CreateActionStack(item.Value.args.card,(Beast) item.Value.args.caster,item.Value.args.isPlayer);
                b.ConditionFailed();
                //activate bad thing here.
            }
            
        }
    }

    public void LeaveBattle()
    {
        inBattle = false;
        foreach (var item in howMuchDamagePlayerDidPerTurn)
        {
            Debug.Log("Player did " +item.Value + "Damage on turn " + item.Key);
        }
        howMuchDamageEnemyDidPerTurn.Clear();
        howMuchDamagePlayerDidPerTurn.Clear();
        EventManager.inst.onBattleEnd.Invoke();
        turn = 0;
      CardManager.inst.  cardsUsedThisTurn.Clear();
        CardStack.inst.Wipe();
        PetManager.inst.LeaveBattle();
        PlayerManager.inst.movement.ReactivateMove();
        WorldViewManager.inst.LeaveBattle();
        BottomPanel.inst.ChangeState(BottomPanel.inst.dialog);
        BottomLeftPanel.inst.SwapToOverworld();
        LeftPanel.inst.SwapToOverworld();
        MusicManager.inst.ChangeToDungeon();
        CardManager.inst.LeaveBattle();
        ManaManager.inst.LeaveBattle();
        EnemyAI.inst.LeaveBattle();
        RivalBeastManager.inst.Wipe();
        UpperLeftPanel.inst.  SwapToOverworld();
        BottomCornerBeastDisplayer.inst.ToggleBattleBGOn();
        AudioManager.inst.GetSoundEffect().Play(leave);
        BattleTicker.inst.Type(LocationManager.inst.currentLocation.locationName);
        Inventory.inst.DisableItemDragOnAll();
        RightPanelButtonManager.inst.SwapToOverworld();
    }

    public EntityOwnership GetBeastOwnership(Beast b)
    {
        if(PlayerParty.inst.party.Contains(b)){
            return EntityOwnership.PLAYER;
        }
        else if(RivalBeastManager.inst.currentParty.Contains(b)){
            return EntityOwnership.ENEMY;
        }
        else
        {
            Debug.LogAssertion("Could not find ownership of " + b.scriptableObject.beastData.beastName);
            return EntityOwnership.ERROR;
        }
    }

}