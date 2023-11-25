 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
public enum EntityOwnership{ERROR,PLAYER,ENEMY}
public class BattleManager:Singleton<BattleManager>                   
{
    public class TurnRecord
    {
        public int turn;
        public List<Card> cardsPlayedThisTurn = new List<Card>();
        public List<CardIntPair> damageDealtByEachCard = new List<CardIntPair>();
        public int bleedDamage;
        public class CardIntPair{
            public Card card;
            public int v;
            public int castOrder;
        }
        

        public int GetAllDamageDealtThisTurn()
        {
            int i = 0;
            foreach (var item in damageDealtByEachCard)
            {i+=item.v;}
            i+= bleedDamage;
            return i;
        }

        public int GetDamageAfterSpecificPoint(int indexPromiseCardWasCastOn)
        {
            int q = 0;
            
            for (int i = indexPromiseCardWasCastOn; i <cardsPlayedThisTurn.Count; i++)
            {
                foreach (var item in damageDealtByEachCard)
                {
                    if(item.castOrder == i){
                        q+= item.v;
                    }
                }
            }
            q+=bleedDamage;
            return q;
        }

    }

    public class QueuedAction{
        public UnityAction action;
        public EffectArgs args;
    }


    public bool FUCKOFF;
    public enum TurnState{Player,Enemy}
    public TurnState turnState;
    public bool inBattle;
    public RectTransform castOrderHolder;
    public int turn;
    public int startingMana;
    public Entity enemyTarget;
    public SoundData leave;
    public Entity playerTarget;
    public StatusEffectHandler statusEffectHandlerPrefab;
    public Dictionary<int,TurnRecord> playerRecord = new Dictionary<int,TurnRecord>();
    
    public Dictionary<int,TurnRecord> enemyRecord = new Dictionary<int,TurnRecord>();
    public Queue<QueuedAction> effectsToUse = new Queue<QueuedAction>();
    public DamageValueGraphic damageValueGraphicPrefab;
    bool statusEffectShit;
    
    public void StartBattle(BattleType battleType)
    {
        inBattle = true;
        turn = 1;
        TurnRecord e = new TurnRecord();
         TurnRecord p = new TurnRecord();
         e.turn = turn;
         p.turn = turn;
        enemyRecord.Add(turn,e);
        playerRecord.Add(turn,p);
        turnState = TurnState.Player;

        if(battleType == BattleType.Wild)
        {
            RivalBeastManager.inst.CreateEnemyParty(WildEncounterManager.inst.GetEncounter());
            RivalBeastManager.inst.SwapActiveBeast(RivalBeastManager.inst.currentParty[0]);
        }
        EndTurnButton.inst.Reactivate();
        BattleField.inst.SetPlayerBeastIcon(PlayerParty.inst.activeBeast);
        BattleField.inst.Init();
        UpperLeftPanel.inst.SwapToBattle();
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

    public bool queuedEffect(){
        return effectsToUse.Count > 0;
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
            if(PetManager.inst.enemyPet!= null && !PetManager.inst.enemyPet.KO)
            {
               
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
     
        BattleTicker.inst.Type("The beast readies itself.");
        CardManager.inst.DeactivateHand();
       
        EndTurnButton.inst.Deactivate();        
        Inventory.inst.DeactivateDrag();
        EnemyAI.inst.Act(RivalBeastManager.inst.activeBeast);
    }

    public void SwapToPlayerTurn()
    {
        EventManager.inst.onNewPlayerTurn.Invoke();
        CardManager.inst.DeactivateHand();
        Inventory.inst.ActivateDrag();
        if(turn % 2 == 0)
        {
            ManaManager.inst.IncreaseMaxMana();
            EnemyAI.inst.IncreaseMaxMana();
        }
        ManaManager.inst.RegenMana();
        EnemyAI.inst.RegenMana();
       IncrementTurn();
      

        StartCoroutine(q());
        IEnumerator q()
        {
            while(statusEffectShit)
            {yield return null;}
            CardManager.inst.DrawCard(); //bug?
        }
    }
    
    void IncrementTurn()
    {
        turn++;
        TurnRecord e = new TurnRecord();
         TurnRecord p = new TurnRecord();
         e.turn = turn;
         p.turn = turn;
        enemyRecord.Add(turn,e);
        playerRecord.Add(turn,p);
        CardStack.inst.NewTurn();
        CheckForBadEffects();
        LoadStatusEffects();
       CheckForStatusEffectBeforeAllowingCards();
    }

    public void CheckForStatusEffectBeforeAllowingCards()
    {
        statusEffectShit = true;
        TriggerQueuedEffects();
        StartCoroutine(w());
        IEnumerator w()
        {
            while(BattleManager.inst.queuedEffect())
            {yield return null;}
            yield return new WaitForSeconds(.5f);
            statusEffectShit = false;
            EndTurnButton.inst.Reactivate();
            CardManager.inst.MakeHandInteractable();
            CardManager.inst.ActivateHand();
            BattleTicker.inst.Type("Make your move");
        }
    }


    public void TriggerQueuedEffects()
    {
        if(queuedEffect())
        {
            StartCoroutine(w());
            IEnumerator w()
            { 
                yield return new WaitForSeconds(.8f);
                QueuedAction qa = effectsToUse.Dequeue();
                qa.action.Invoke();
                BattleTicker.inst.Type(qa.args.tickerTitle);
                if(CheckIfGameContinues()){
TriggerQueuedEffects();
                }
                
            }
        }
    }

    float LoadStatusEffects()
    {
        p(PlayerParty.inst.party);
        p(RivalBeastManager.inst.currentParty);
        void p(List<Beast> b)
        {
            foreach (var item in b)
            {
                if(item.statusEffectHandler != null){
                    foreach (var d in item.statusEffectHandler.displays)
                    {
                        if(d.statusEffect == StatusEffects.POISON)
                        {
                           // Debug.Log("og + " + d.scriptableObject.statusEffect.ToString());
                            
                            QueuedAction qa = new QueuedAction();
                            UnityAction ua =()=>    d.Trigger();
                            qa.action = ua;
                            string m =d.scriptableObject.statusEffect.ToString();
                            qa.args =  new EffectArgs(null,null,false,null,null,-55, MiscFunctions.FirstLetterToUpperCaseOrConvertNullToEmptyString(m));
                            effectsToUse.Enqueue(qa);
                        }
                    }
                }
            }
        }
       
        return .25f;
    }

    void CheckForBadEffects()
    {

        foreach (var item in CardManager.inst.promiseDict)
        {
            if(item.Value.turnToDieOn == turn)
            {
                if(item.Value.promise.badEffects.Count != 0)
                {
                  UnityAction a = ()=>  item.Value.promise.ExecuteBadEffects(item.Value.args);
                  QueuedAction qa = new QueuedAction();
                  qa.action = a;
                  qa.args = item.Value.args;
                  effectsToUse.Enqueue(qa);
                }
                Debug.Log("Remove " + item.Value);
                foreach (var e in item.Value.subbedEvents)
                {EventManager.inst.RemoveEvent(e,item.Value.action);}
                CardStackBehaviour b =  CardStack.inst.CreateActionStack(item.Value.args.card,(Beast) item.Value.args.caster,item.Value.args.isPlayer);
                b.ConditionFailed();
                
                if( item.Value.promise. unStackable){
                CardManager.inst.promiseList.Remove(item.Value.promise);
                }
               
                //activate bad thing here.
            }
            
        }
    }

    public void LeaveBattle()
    {
        inBattle = false;
        // foreach (var item in howMuchDamagePlayerDidPerTurn)
        // {
        //     Debug.Log("Player did " +item.Value + "Damage on turn " + item.Key);
        // }
        playerRecord.Clear();
        enemyRecord.Clear();
        EventManager.inst.onBattleEnd.Invoke();
        turn = 0;
        effectsToUse.Clear();
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

    // public EntityOwnership GetBeastOwnership(Beast b)
    // {
    //     if(PlayerParty.inst.party.Contains(b)){
    //         return EntityOwnership.PLAYER;
    //     }
    //     else if(RivalBeastManager.inst.currentParty.Contains(b)){
    //         return EntityOwnership.ENEMY;
    //     }
    //     else
    //     {
    //         Debug.LogAssertion("Could not find ownership of " + b.scriptableObject.beastData.beastName);
    //         return EntityOwnership.ERROR;
    //     }
    // }

}