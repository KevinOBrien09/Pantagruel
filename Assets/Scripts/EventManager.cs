using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;


public class EventManager:Singleton<EventManager>
{
   
    public UnityEvent onStep;
    public UnityEvent onBattleStart;
    public UnityEvent onBattleEnd;
    public UnityEvent onNewTurn;
    public UnityEvent onNewPlayerTurn;
    public UnityEvent onPlayerDrawingCardFirstTimeInTurn;
    public UnityEvent onNewEnemyTurn;
    // public UnityEvent onCardDraw;
    // public UnityEvent onCardUse;
    public UnityEvent onPlayerCastCard;
    public UnityEvent onPlayerDrawCard;
    public UnityEvent onEnemyDrawCard;
    public UnityEvent onEnemyCastCard;
    public UnityEvent onPlayerUseItem;
    public UnityEvent onPlayerVapourCardCreation;
    public UnityEvent onPetDeath;
    public UnityEvent onPlayerPetDeath;
    public UnityEvent onEnemyPetDeath;
    public UnityEvent onPetSummon;
    public UnityEvent onPlayerPetSummon;
    public UnityEvent onEnemyPetSummon;
    public UnityEvent onEnemyBeastDeath;
    public UnityEvent onEnemyPetKilledByPlayer;
    public UnityEvent onPlayerPetKilledByPlayer;
    public UnityEvent onEnemyPetKilledByEnemy;
    public UnityEvent onPlayerPetKilledByEnemy;
   
    public UnityEvent onPlayerDealDamage;
    public UnityEvent onEnemyDealDamage;

    public UnityEvent onEnemyBeastKilledByPlayer;
    public UnityEvent onEnemyBeastKilledByEnemy;
    public UnityEvent onPlayerBeastKilledByEnemy;
    public UnityEvent onPlayerBeastKilledByPlayer;

    public UnityEvent onPlayerEnterGuardState;
    public UnityEvent onPlayerExitGuardState;
    public UnityEvent onPlayerGuardBreak;
    
    public UnityEvent onEnemyEnterGuardState;
    public UnityEvent onEnemyExitGuardState;
    public UnityEvent onEnemyGuardBreak;

    public void AssignEvent(EventEnum eventEnum,UnityAction action)
    {
        switch(eventEnum)
        {
            case EventEnum.onStep:
            onStep.AddListener(action);
            break;
            case EventEnum.onBattleStart:
            onBattleStart.AddListener(action);
            break;
            case EventEnum.onBattleEnd:
            onBattleEnd.AddListener(action);
            break;
            case EventEnum.onNewTurn:
            onNewTurn.AddListener(action);
            break;
            case EventEnum.onNewPlayerTurn:
            onNewPlayerTurn.AddListener(action);
            break;
            case EventEnum.onPlayerDrawingCardFirstTimeInTurn:
            onPlayerDrawingCardFirstTimeInTurn.AddListener(action);
            break;
            case EventEnum.onNewEnemyTurn:
            onNewEnemyTurn.AddListener(action);
            break;
            case EventEnum.onPlayerCastCard:
            onPlayerCastCard.AddListener(action);
            break;
            case EventEnum.onPlayerDrawCard:
            onPlayerDrawCard.AddListener(action);
            break;
            case EventEnum.onEnemyDrawCard:
            onEnemyDrawCard.AddListener(action);
            break;
            case EventEnum.onEnemyCastCard:
            onEnemyCastCard.AddListener(action);
            break;
            case EventEnum.onPlayerUseItem:
            onPlayerUseItem.AddListener(action);
            break;
            case EventEnum.onPetDeath:
            onPetDeath.AddListener(action);
            break;
            case EventEnum.onPlayerPetDeath:
            onPlayerPetDeath.AddListener(action);
            break;
            case EventEnum.onEnemyPetDeath:
            onEnemyPetDeath.AddListener(action);
            break;
            case EventEnum.onPetSummon:
            onPetSummon.AddListener(action);
            break;
            case EventEnum.onPlayerPetSummon:
            onPlayerPetSummon.AddListener(action);
            break;
            case EventEnum.onEnemyPetSummon:
            onEnemyPetSummon.AddListener(action);
            break;
            case EventEnum.onEnemyBeastDeath:
            onEnemyBeastDeath.AddListener(action);
            break;
            case EventEnum.onEnemyPetKilledByPlayer:
            onEnemyPetKilledByPlayer.AddListener(action);
            break;
            case EventEnum.onPlayerPetKilledByPlayer:
            onPlayerPetKilledByPlayer.AddListener(action);
            break;
            case EventEnum.onPlayerPetKilledByEnemy:
            onPlayerPetKilledByEnemy.AddListener(action);
            break;
            case EventEnum.onEnemyPetKilledByEnemy:
            onEnemyPetKilledByEnemy.AddListener(action);
            break;
            case EventEnum.onPlayerDealDamage:
            onPlayerDealDamage.AddListener(action);
            break;
            case EventEnum.onEnemyDealDamage:
            onEnemyDealDamage.AddListener(action);
            break;
            case EventEnum.onEnemyBeastKilledByPlayer:
            onEnemyBeastKilledByPlayer.AddListener(action);
            break;
            case EventEnum.onEnemyBeastKilledByEnemy:
            onEnemyBeastKilledByEnemy.AddListener(action);
            break;
            case EventEnum.onPlayerBeastKilledByEnemy:
            onPlayerBeastKilledByEnemy.AddListener(action);
            break;
            case EventEnum.onPlayerBeastKilledByPlayer:
            onPlayerBeastKilledByPlayer.AddListener(action);
            break;

              case EventEnum.onPlayerEnterGuardState:
            onPlayerEnterGuardState.AddListener(action);
            break;
            case EventEnum.onPlayerExitGuardState:
            onPlayerExitGuardState.AddListener(action);
            break;
            case EventEnum.onPlayerGuardBreak:
           onPlayerGuardBreak.AddListener(action);
            break;
            case EventEnum.onEnemyEnterGuardState:
            onEnemyEnterGuardState.AddListener(action);
            break;
            case EventEnum.onEnemyExitGuardState:
            onEnemyExitGuardState.AddListener(action);
            break;
            case EventEnum.onEnemyGuardBreak:
            onEnemyGuardBreak.AddListener(action);
            break;
        
        }
    }


    public void RemoveEvent(EventEnum eventEnum,UnityAction action)
    {
        switch(eventEnum)
        {
            case EventEnum.onStep:
            onStep.RemoveListener(action);
            break;
            case EventEnum.onBattleStart:
            onBattleStart.RemoveListener(action);
            break;
            case EventEnum.onBattleEnd:
            onBattleEnd.RemoveListener(action);
            break;
            case EventEnum.onNewTurn:
            onNewTurn.RemoveListener(action);
            break;
            case EventEnum.onNewPlayerTurn:
            onNewPlayerTurn.RemoveListener(action);
            break;
            case EventEnum.onPlayerDrawingCardFirstTimeInTurn:
            onPlayerDrawingCardFirstTimeInTurn.RemoveListener(action);
            break;
            case EventEnum.onNewEnemyTurn:
            onNewEnemyTurn.RemoveListener(action);
            break;
            case EventEnum.onPlayerCastCard:
            onPlayerCastCard.RemoveListener(action);
            break;
            case EventEnum.onPlayerDrawCard:
            onPlayerDrawCard.RemoveListener(action);
            break;
            case EventEnum.onEnemyDrawCard:
            onEnemyDrawCard.RemoveListener(action);
            break;
            case EventEnum.onEnemyCastCard:
            onEnemyCastCard.RemoveListener(action);
            break;
            case EventEnum.onPlayerUseItem:
            onPlayerUseItem.RemoveListener(action);
            break;
            case EventEnum.onPetDeath:
            onPetDeath.RemoveListener(action);
            break;
            case EventEnum.onPlayerPetDeath:
            onPlayerPetDeath.RemoveListener(action);
            break;
            case EventEnum.onEnemyPetDeath:
            onEnemyPetDeath.RemoveListener(action);
            break;
            case EventEnum.onPetSummon:
            onPetSummon.RemoveListener(action);
            break;
            case EventEnum.onPlayerPetSummon:
            onPlayerPetSummon.RemoveListener(action);
            break;
            case EventEnum.onEnemyPetSummon:
            onEnemyPetSummon.RemoveListener(action);
            break;
            case EventEnum.onEnemyBeastDeath:
            onEnemyBeastDeath.RemoveListener(action);
            break;
            case EventEnum.onEnemyPetKilledByPlayer:
            onEnemyPetKilledByPlayer.RemoveListener(action);
            break;
               case EventEnum.onPlayerPetKilledByPlayer:
            onPlayerPetKilledByPlayer.RemoveListener(action);
            break;
               case EventEnum.onPlayerPetKilledByEnemy:
           onPlayerPetKilledByEnemy.RemoveListener(action);
            break;
               case EventEnum.onEnemyPetKilledByEnemy:
          onEnemyPetKilledByEnemy.RemoveListener(action);
            break;
               case EventEnum.onPlayerDealDamage:
            onPlayerDealDamage.RemoveListener(action);
            break;
               case EventEnum.onEnemyDealDamage:
            onEnemyDealDamage.RemoveListener(action);
            break;
            case EventEnum.onEnemyBeastKilledByPlayer:
            onEnemyBeastKilledByPlayer.RemoveListener(action);
            break;
            case EventEnum.onEnemyBeastKilledByEnemy:
            onEnemyBeastKilledByEnemy.RemoveListener(action);
            break;
            case EventEnum.onPlayerBeastKilledByEnemy:
            onPlayerBeastKilledByEnemy.RemoveListener(action);
            break;
            case EventEnum.onPlayerBeastKilledByPlayer:
            onPlayerBeastKilledByPlayer.RemoveListener(action);
            break;

            case EventEnum.onPlayerEnterGuardState:
            onPlayerEnterGuardState.RemoveListener(action);
            break;
            case EventEnum.onPlayerExitGuardState:
            onPlayerExitGuardState.RemoveListener(action);
            break;
            case EventEnum.onPlayerGuardBreak:
           onPlayerGuardBreak.RemoveListener(action);
            break;
            case EventEnum.onEnemyEnterGuardState:
            onEnemyEnterGuardState.RemoveListener(action);
            break;
            case EventEnum.onEnemyExitGuardState:
            onEnemyExitGuardState.RemoveListener(action);
            break;
            case EventEnum.onEnemyGuardBreak:
            onEnemyGuardBreak.RemoveListener(action);
            break;
        
        }
    }
}

public enum EventEnum
    {
        onStep,
        onBattleStart,
        onBattleEnd,
        onNewTurn,
        onNewPlayerTurn,
        onPlayerDrawingCardFirstTimeInTurn,
        onNewEnemyTurn,
        onPlayerCastCard,
        onPlayerDrawCard,
        onEnemyDrawCard,
        onEnemyCastCard,
        onPlayerUseItem,
        onPlayerVapourCardCreation,
        onPetDeath,
        onPlayerPetDeath,
        onEnemyPetDeath,
        onPetSummon,
        onPlayerPetSummon,
        onEnemyPetSummon,
        onEnemyBeastDeath,
        onEnemyPetKilledByPlayer,
        onPlayerPetKilledByPlayer,
        onEnemyPetKilledByEnemy,
        onPlayerPetKilledByEnemy,
      
        onPlayerDealDamage,
        onEnemyDealDamage,
        onEnemyBeastKilledByPlayer,
        onEnemyBeastKilledByEnemy,
        onPlayerBeastKilledByEnemy,
        onPlayerBeastKilledByPlayer,
        onPlayerEnterGuardState,
        onPlayerExitGuardState,
        onPlayerGuardBreak,

        onEnemyEnterGuardState,
        onEnemyExitGuardState,
        onEnemyGuardBreak
    }