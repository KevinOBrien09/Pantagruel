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

   

   
}