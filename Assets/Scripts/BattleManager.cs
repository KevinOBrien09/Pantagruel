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
    [System.Serializable]
    public class TurnRecord
    {
        public int turn;
        public List<Card> cardsPlayedThisTurn = new List<Card>();
        public int totalTurnDamage;
        

       
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
    public SoundData leave,enter;
    public Entity playerTarget;
    public StatusEffectHandler statusEffectHandlerPrefab;
    public GenericDictionary<int,TurnRecord> playerRecord = new GenericDictionary<int,TurnRecord>();
    public Image passiveProcImage;
    public SoundData passiveProcSFX;
    public List<SoundData> dodgeSFX = new List<SoundData>();
    public GenericDictionary<int,TurnRecord> enemyRecord = new GenericDictionary<int,TurnRecord>();
    public Queue<QueuedAction> effectsToUse = new Queue<QueuedAction>();
    public DamageValueGraphic damageValueGraphicPrefab;
    public BattleTextBehaviour battleTextBehaviourPrefab;
    bool statusEffectShit;
    public bool inTutorial;
    public bool doNotTurnOnEndTurnButton;
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
             AngelStage.inst.Toggle();
        if(battleType == BattleType.Wild)
        {
            RivalBeastManager.inst.CreateEnemyParty(WildEncounterManager.inst.GetEncounter());
            RivalBeastManager.inst.SwapActiveBeast(RivalBeastManager.inst.currentParty[0]);
        }
        EndTurnButton.inst.Deactivate();
          BattleField.inst.SetPlayerBeastIcon(PlayerParty.inst.activeBeast);
        BattleField.inst.Init();
        UpperLeftPanel.inst.SwapToBattle();
        SetEnemyTarget(RivalBeastManager.inst.activeBeast);
        SetPlayerTarget(PlayerParty.inst.activeBeast);
            EnemyAI.inst.manaHandler.SwapBeast(RivalBeastManager.inst.activeBeast);
        CardManager.inst.manaHandler.SwapBeast(PlayerParty.inst.activeBeast);
       
        LeftPanel.inst.SwapToBattle();
        BottomLeftPanel.inst.SwapToBattle();
        BottomCornerBeastDisplayer.inst.ToggleBattleBGOff();
       
        RightPanelButtonManager.inst.SwapToBattle();
        CardStack.inst.EnterBattle();
        
        bool trainer = false;
        if(trainer){
            BattleIntro.inst.EnterBattle(PlayerParty.inst.activeBeast,RivalBeastManager.inst.activeBeast);
        }else
        {
            BattleTicker.inst.Type("The Ophanim");
            WorldViewManager.inst.EnterBattle();
            StartCoroutine(q());
            IEnumerator q()
            {
                AudioManager.inst.GetSoundEffect().Play(enter);
                if(!TutorialManager.inst.ExecuteTutorial(TutorialEnum.BASICS))
                {
                    MusicManager.inst.ChangeMusic(LocationManager.inst.currentSubLocation.battleMusic);
                }
                else{
                    MusicManager.inst.Silent();

                }
                yield   return new WaitForSeconds(2f);
                BattleTicker.inst.Type("Turn " +  BattleManager.inst.turn.ToString());
                EnterBattlePartTwo();
            }
        }
    }
    
    public void EnterBattlePartTwo()
    {
       
        
        // for (int i = 0; i < startingMana; i++)
        // {   
        //     ManaManager.inst.IncreaseMaxMana(); 
        //     EnemyAI.inst.IncreaseMaxMana();
        // }
        // ManaManager.inst.RegenMana();
        // EnemyAI.inst.RegenMana();
    
        EnemyAI.inst.DrawRandomCard();
        EnemyAI.inst.DrawRandomCard();
        EnemyAI.inst.DrawRandomCard();
        //   /CardManager.inst.EnterBattle(PlayerParty.inst.activeBeast);
        if(TutorialManager.inst.ExecuteTutorial(TutorialEnum.BASICS))
        {
       
          
            TutorialTextTime(TutorialManager.inst.GetTutorial(TutorialEnum.BASICS));
            
            
        }
        else
        {
           
            BottomPanel.inst.ChangeState(BottomPanel.inst.cards);
            CardManager.inst.EnterBattle(PlayerParty.inst.activeBeast);
            EndTurnButton.inst.Reactivate();
            Inventory.inst.EnableItemDragOnAll();
           
            EventManager.inst.onBattleStart.Invoke();
        }
    }
    
    public void TutorialTextTime(Dialog tutorial)
    { 
        inTutorial = true;
        BottomPanel.inst.ChangeState(BottomPanel.inst.dialog);
        DialogManager.inst.StartConversation(tutorial);
       
        CardManager.inst.HideHand();
        Inventory.inst.DisableItemDragOnAll();
        EndTurnButton.inst.Deactivate();
    }

    public void ExitTutorial(TutorialEnum tutorialEnum){
        switch (tutorialEnum)
        {
            case TutorialEnum.BASICS:
          //  MusicManager.inst.EnterBattle();
            BottomPanel.inst.ChangeState(BottomPanel.inst.cards);
           
            
            CardManager.inst.TutorialStart(PlayerParty.inst.activeBeast);
            //CardManager.inst.EnterBattle(PlayerParty.inst.activeBeast);
            DialogManager.inst.HidePotraits();
            CardManager.inst.ActivateHand();
            EndTurnButton.inst.Reactivate();
            Inventory.inst.EnableItemDragOnAll();
            EventManager.inst.onBattleStart.Invoke();
            return;

            case TutorialEnum.BASICS2:
            BottomPanel.inst.ChangeState(BottomPanel.inst.cards);
            DialogManager.inst.HidePotraits();
            CheckForStatusEffectBeforeAllowingCards();
            break;

            case TutorialEnum.BASICS3:
            
            BottomPanel.inst.ChangeState(BottomPanel.inst.cards);
            DialogManager.inst.HidePotraits();
            string guardID = "a81f0e7c-ab70-464e-995e-f02f199d30ad";
            string slashID = "cafdbe56-857e-454d-9cf2-a8c07371f4cf";
            CardManager.inst.CreateCardBehaviour(  CardManager.inst.DrawSpecificCard(slashID));
            CardManager.inst.CreateCardBehaviour(  CardManager.inst.DrawSpecificCard(guardID));
           // doNotTurnOnEndTurnButton = true;
            CheckForStatusEffectBeforeAllowingCards();
         
            break;

            case TutorialEnum.BASICS4:
            
            BottomPanel.inst.ChangeState(BottomPanel.inst.cards);
            DialogManager.inst.HidePotraits();
            string slashIDq = "cafdbe56-857e-454d-9cf2-a8c07371f4cf";
            CardManager.inst.CreateCardBehaviour(  CardManager.inst.DrawSpecificCard(slashIDq));
            // string guardID = "a81f0e7c-ab70-464e-995e-f02f199d30ad";
            // string slashID = "cafdbe56-857e-454d-9cf2-a8c07371f4cf";
            // CardManager.inst.CreateCardBehaviour(  CardManager.inst.DrawSpecificCard(slashID));
            // CardManager.inst.CreateCardBehaviour(  CardManager.inst.DrawSpecificCard(guardID));
           // doNotTurnOnEndTurnButton = true;
            CheckForStatusEffectBeforeAllowingCards();
            EndTurnButton.inst.Deactivate();
         
                break;
            
            default:
            Debug.LogAssertion("DEFAULT CASE OWOWOWOWWOW");
            break;
        }

        inTutorial = false;
    }

    public bool CheckIfGameContinues()
    {
        if(!inBattle){
            return false;
        }
        bool playerActiveBeastIsDead = PlayerParty.inst.activeBeast.currentHealth <= 0;
        bool enemyActiveDeastIsDead = RivalBeastManager.inst.activeBeast.currentHealth <= 0;
        
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
        return true;
    }

    public bool queuedEffect()
    {return effectsToUse.Count > 0;}

   

    public void SetEnemyTarget(Entity e)
    {enemyTarget = e;}

    public void SetPlayerTarget(Entity e)
    {playerTarget = e;}

    public void Win()
    {
        Debug.Log("Win");
        CardManager.inst.DeactivateHand();
        if(!TutorialManager.inst.DEBUG)
        {
            if(TutorialManager.inst.currentTutorial == TutorialManager.inst.GetTutorial(TutorialEnum.BASICS4))
            {
                StartCoroutine(a());
                IEnumerator a()
                {
                    EndTurnButton.inst.Deactivate();
                    CardManager.inst.DeactivateHand();
                    foreach (var item in CardManager.inst.hand)
                    {item.VaporousDissolve();}
                    inBattle = false;
                    MusicManager.inst.ChangeMusic(LocationManager.inst.currentSubLocation.overworldMusic);
                    yield return new WaitForEndOfFrame();
                    if(PetManager.inst.enemyPet!= null && !PetManager.inst.enemyPet.KO)
                    {PetManager.inst.enemyPet.Die(EntityOwnership.ERROR);}
                
                    yield return new WaitForSeconds(1.5f);
                    LeaveBattle();
                    DialogManager.inst.  ToggleButtons(true);
                    Interactor.inst.RenableInteraction();
                    TutorialManager.inst.LeaveFirstBattle();

                }
            
                Debug.Log("XD");
                return;
            }
        }
        
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
            {PetManager.inst.enemyPet.Die(EntityOwnership.ERROR);}
         
            yield return new WaitForSeconds(1.5f);
            RewardManager.inst.Open();
           // LeaveBattle();
        }
    }

    public void PassiveProc(Beast b)
    {
        passiveProcImage.sprite = b.scriptableObject.beastData.uiPicture;
        passiveProcImage.DOFade(.2f,.5f).OnComplete(()=>passiveProcImage.DOFade(0,.5F));
        AudioManager.inst.GetSoundEffect().Play(passiveProcSFX);
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
            TutorialManager.inst.ProcessEvent("@");
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

    public void RecordPlayerDamage(int dmg)
    {playerRecord[turn].totalTurnDamage +=dmg;}

    public void RecordEnemyDamage(int dmg)
    {enemyRecord[turn].totalTurnDamage +=dmg;}

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
        if(inBattle)
        {
            
            CardManager.inst.DeactivateHand();
            Inventory.inst.ActivateDrag();
            EnemyAI.inst.manaHandler.Regen(RivalBeastManager.inst.activeBeast.stats().manaRegen);
            CardManager.inst.manaHandler.Regen(PlayerParty.inst.activeBeast.stats().manaRegen);
            // if(turn % 2 == 0)
            // {
            //     ManaManager.inst.IncreaseMaxMana();
            //     EnemyAI.inst.IncreaseMaxMana();
            // }
            // ManaManager.inst.RegenMana();
            // EnemyAI.inst.RegenMana();
            IncrementTurn();
        

            if(TutorialManager.inst.ExecuteTutorial(TutorialEnum.BASICS3))
            {
                TutorialTextTime(TutorialManager.inst.GetTutorial(TutorialEnum.BASICS3));

                return;
            }
            else if(TutorialManager.inst.ExecuteTutorial(TutorialEnum.BASICS4)){
 TutorialTextTime(TutorialManager.inst.GetTutorial(TutorialEnum.BASICS4));
            }
           
            else{
                StartCoroutine(q());
            }


           
            IEnumerator q()
            {
                while(statusEffectShit)
                {yield return null;}
                
                Card c = null;
                if(!CardManager.inst.blockade)
                {
                    if(!CardManager.inst.currentTurnDrawIsPredeterminded())
                    { c = CardManager.inst.DrawCard();}
                    else
                    { c = CardManager.inst.DrawPredeterminedCard();}

                }
              
                if(c != null)
                {
                    CardManager.inst.CreateCardBehaviour(c); 
                }
                CardManager.inst.MakeHandInteractable();
            }
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
        EventManager.inst.onNewPlayerTurn.Invoke();
        CardStack.inst.NewTurn();

        if(TutorialManager.inst.ExecuteTutorial(TutorialEnum.BASICS3))
        {
            TutorialTextTime(TutorialManager.inst.GetTutorial(TutorialEnum.BASICS3));
            return;
        }
          
        CheckForBadEffects();
        LoadStatusEffects();
        CheckForStatusEffectBeforeAllowingCards();
        CheckStatMods(PlayerParty.inst.party);
        CheckStatMods(RivalBeastManager.inst.currentParty);
    }

    public void CheckStatMods(List<Beast> e )
    {
        foreach (var beast in e)
        {
            List<StatMod> bin = new List<StatMod>();
            foreach (var mod in beast.mods)
            {
                if(!mod.untilEndOfCombat){
                    if(turn == mod.turnToDieOn)
                    {
                        StatMod m = new StatMod();
                        m.stat = mod.stat;
                        m.change = Maths.ConvertToNegativeAndPostive(mod.change);
                        beast.statMods.ModStat(m);
                        bin.Add(mod);
                    }
                }
                else
                {
                    if(!inBattle)
                    {
                        StatMod m = new StatMod();
                        m.stat = mod.stat;
                        m.change = Maths.ConvertToNegativeAndPostive(mod.change);
                        beast.statMods.ModStat(m);
                        bin.Add(mod);
                    }
                }
            }
            foreach (var mod in bin)
            {beast.mods.Remove(mod);}
        }
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
   
            CardManager.inst.MakeHandInteractable();
            CardManager.inst.ActivateHand();
            yield return new WaitForSeconds(.5f);
            BattleTicker.inst.Type("Make your move");
            yield return new WaitForSeconds(.5f);
            EndTurnButton.inst.Reactivate();

            
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
                            qa.args =  new EffectArgs(null, null,null,null,-55, MiscFunctions.FirstLetterToUpperCaseOrConvertNullToEmptyString(m));
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
                  UnityAction a = ()=>  item.Value.promise.ExecuteBadEffects(item.Value.args,item.Key);
                  QueuedAction qa = new QueuedAction();
                  qa.action = a;
                  qa.args = item.Value.args;
                  effectsToUse.Enqueue(qa);
                }
                Debug.Log("Remove " + item.Value);
                foreach (var e in item.Value.subbedEvents)
                {EventManager.inst.RemoveEvent(e,item.Value.action);}
                CardStackBehaviour b =  CardStack.inst.CreateActionStack(item.Value.args.card,(Beast) item.Value.args.caster);
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
        CheckStatMods(PlayerParty.inst.party);
        playerRecord.Clear();
        enemyRecord.Clear();
        EventManager.inst.onBattleEnd.Invoke();
        turn = 0;
        effectsToUse.Clear();
        DamageTracker.inst.Wipe();
        PlayerManager.inst.movement.ResetPOV();
        CardStack.inst.Wipe();
        PetManager.inst.LeaveBattle();
        PlayerParty.inst.RemoveStatusEffectsEndOfCombat();
        PlayerManager.inst.movement.ReactivateMove();
        WorldViewManager.inst.LeaveBattle();
        BottomPanel.inst.ChangeState(BottomPanel.inst.dialog);
        BottomLeftPanel.inst.SwapToOverworld();
        LeftPanel.inst.SwapToOverworld();
        MusicManager.inst.ChangeMusic(LocationManager.inst.currentSubLocation.overworldMusic);
        CardManager.inst.LeaveBattle();
        EnemyAI.inst.manaHandler.Wipe();
        CardManager.inst.manaHandler.Wipe();
        EnemyAI.inst.LeaveBattle();
        RivalBeastManager.inst.Wipe();
        UpperLeftPanel.inst.  SwapToOverworld();
        BottomCornerBeastDisplayer.inst.ToggleBattleBGOn();
        AudioManager.inst.GetSoundEffect().Play(leave);
        BattleTicker.inst.Type(LocationManager.inst.currentSubLocation.locationName);
        Inventory.inst.DisableItemDragOnAll();
        RightPanelButtonManager.inst.SwapToOverworld();
         AngelStage.inst.Toggle();
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