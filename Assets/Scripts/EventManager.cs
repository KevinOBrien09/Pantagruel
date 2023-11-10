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
        onEnemyBeastDeath
    }