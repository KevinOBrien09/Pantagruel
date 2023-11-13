using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;
using DG.Tweening;
using System;
public class ActionEventPair{
    public string ID;
    public UnityAction action;
    public List<EventEnum> subbedEvents = new List<EventEnum>();
    public Promise promise;
    public EffectArgs args;
    public int turnCastOn;
    public int turnToDieOn;
}

public class CardManager:Singleton<CardManager>                   
{
    public CardBehaviour prefab;
    public RectTransform holder;
    public List<CardBehaviour> hand = new List<CardBehaviour>();
   
    float holderOgPos;
    public float downHodlerPos;
    public Deck currentDeck;
    public Beast currentBeast;
    public SoundData drawCard;
    Dictionary<Beast,Deck> deckDict = new Dictionary<Beast, Deck>();
    public bool handDown;
    public Dictionary<string,ActionEventPair> promiseDict = new Dictionary<string,ActionEventPair>();
    public List<Promise> promiseList = new List<Promise>();
    
    public void EnterBattle(Beast beast)
    {
        foreach (var item in PlayerParty.inst.party)
        {
            deckDict.Add(item,new Deck());
            deckDict[item].cards = new List<Card>(item.deck.cards);
        }
        
        foreach (var item in hand)
        {Destroy(item.gameObject);}
     
        SwitchBeast(beast);
        ShuffleDeck();
        DrawCard();
        DrawCard();
        DrawCard();
        ActivateHand();
    }

    public void SwitchBeast(Beast beast)
    {
       DestroyHand();
        currentDeck = deckDict[beast];
    }

    void DestroyHand(){
        CheckForHandFuckery();
        List<CardBehaviour> kill = new List<CardBehaviour>();
        foreach (var item in hand)
        {
            currentDeck.discard.Add(item.card);
            kill.Add(item);
        }
        foreach (var item in kill)
        {
            hand.Remove(item);
            Destroy(item.gameObject);
        }
    }

    public void ActivateHand()
    {
        handDown = false;
        holder.DOAnchorPosY(0,.2f).OnComplete(()=>
        {
            MakeHandInteractable();
        });
    }

    public void MakeHandInteractable()
    {
        foreach(CardBehaviour card in hand)
        {  card.EnableInteractable();}
    }
    
    public void DeactivateHand()
    {
        handDown = true;
        holder.DOAnchorPosY(downHodlerPos,.2f);
        foreach(CardBehaviour card in hand)
        {card.DisableInteractable();}
    }

    void ShuffleDeck(){
        System.Random rng = new  System.Random();
        
        var shuffledcards = currentDeck.cards.OrderBy(a => rng.Next()).ToList();
        currentDeck.cards = shuffledcards;
    }

    public void DrawCard()
    {
        if(currentDeck.cards.Count <= 0)
        { currentDeck.ResetDiscardPile(); }
        
        if(hand.Count < 7)
        { CreateCardBehaviour(CardFunctions.DrawRandomCard(currentDeck)); 
        MakeHandInteractable();}
        else
        { Debug.Log("Hand is full!"); }
    }

   
    public void CheckForHandFuckery()
    {
        List<CardBehaviour> fuckery = new List<CardBehaviour>();
        foreach (var item in hand)
        {
            if(item.isVapour){
                fuckery.Add(item);
                item.transform.SetParent(BottomPanel.inst.transform);
                //item.rt.DOAnchorPosY(-85,.2f);
                item.VaporousDissolve();
                StartCoroutine(q());
                IEnumerator q()
                {
                    yield return new WaitForSeconds(1.5f);
                    Destroy(item.gameObject);
                }
            }
        }

        foreach (var item in fuckery)
        {
            hand.Remove(item);
            
        }
    }

    public void CreateVapourCard(Card card)
    {
        if(hand.Count < 7)
        {
            EventManager.inst.onPlayerVapourCardCreation.Invoke();
            CreateCardBehaviour(card,true);
        }

    }


    public void CreateCardBehaviour(Card card,bool isVapour = false)
    {
        if(card != null)
        {
            EventManager.inst.onPlayerDrawCard.Invoke();
            CardBehaviour newCard = Instantiate(prefab,holder);
            newCard.Init(card,isVapour);
            hand.Add(newCard);
            AudioManager.inst.GetSoundEffect().Play(drawCard);
        }
        else
        { Debug.Log("card is null"); }
    }

  
   
   

  
    public void Use(Card usedCard,CardBehaviour behaviour)
    {
        StartCoroutine(q());
        BattleManager.inst.playerRecord[BattleManager.inst.turn].cardsPlayedThisTurn.Add(usedCard);
        EventManager.inst.onPlayerCastCard.Invoke();

        IEnumerator q()
        {
            if(usedCard.castDelay != 0)
            {CardManager.inst.DeactivateHand();}
          
            if(!usedCard.playVFXAfterDelay)
            {BattleEffectManager.inst.Play(usedCard.vfx);}

            if(usedCard.playVFXAfterDelay)
            {
                if(usedCard.vfxSetUp!=string.Empty)
                {BattleEffectManager.inst.Play(usedCard.vfxSetUp);}
            }
            
            if(usedCard.soundEffect.audioClip != null)
            {AudioManager.inst.GetSoundEffect().Play(usedCard.soundEffect); }
            behaviour.gameObject.SetActive(false);

            yield return new WaitForSeconds(usedCard.castDelay);

            if(!behaviour.isVapour)
            {currentDeck.discard.Add(usedCard);}

            if(usedCard.playVFXAfterDelay)
            {BattleEffectManager.inst.Play(usedCard.vfx);}
            BattleTicker.inst.Type(usedCard.cardName);
            ManaManager.inst.Spend(usedCard.manaCost);
            
            RemoveFromHand(behaviour);
            CardStackBehaviour stackBehaviour =  CardStack.inst.CreateActionStack(usedCard,PlayerParty.inst.activeBeast,true);
            foreach (var effect in usedCard.effects)
            { 
                EffectArgs args = new EffectArgs(PlayerParty.inst.activeBeast,BattleManager.inst.enemyTarget,true,usedCard,stackBehaviour,BattleManager.inst.playerRecord[BattleManager.inst.turn].cardsPlayedThisTurn.IndexOf(usedCard));
                effect.Use(args); 
            }
            

            if(usedCard.castDelay != 0)
            {CardManager.inst.ActivateHand();}

            StartCoroutine(piss());
            
            IEnumerator piss()
            {
                yield return new WaitForSeconds(1f);
      
                BattleTicker.inst.Type("Make your move");
            }
        }
    }

    public void RemoveFromHand(CardBehaviour behaviour){
        hand.Remove(behaviour);
        Destroy(behaviour.gameObject);
    }

    public void LeaveBattle()
    {
        foreach (var item in promiseDict)
        {
            foreach (var eventEnum in item.Value.subbedEvents)
            {
               EventManager.inst.RemoveEvent(eventEnum,item.Value.action);
            }
            
        }
        promiseList.Clear();
        promiseDict.Clear();
        DestroyHand();
        deckDict.Clear();
        
        hand.Clear();
        currentBeast = null;
        currentDeck = null;
        DeactivateHand();

      
    }
}