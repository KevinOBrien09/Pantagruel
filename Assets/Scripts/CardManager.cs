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

public class CardSFXPair{
    public Card card;
    public SoundData sfx;
}

public class HitData{
    public Entity entity;
    public bool dodged;
    public List<Effect> effects = new List<Effect>();
}

public class CardManager:Singleton<CardManager>                   
{
    public enum CardState{NORMAL,VAPOUR,MANIFESTED}
    public CardBehaviour prefab;
    public RectTransform holder;
    public List<CardBehaviour> hand = new List<CardBehaviour>();
    public bool blockade;
    float holderOgPos;
    public float downHodlerPos;
    public Deck currentDeck;
    public Beast currentBeast;
    public SoundData drawCard;
    Dictionary<Beast,Deck> deckDict = new Dictionary<Beast, Deck>();
    public bool handDown;
    public Dictionary<string,ActionEventPair> promiseDict = new Dictionary<string,ActionEventPair>();
    public List<Promise> promiseList = new List<Promise>();
    Dictionary<int,CardSFXPair> predetermindedDraws = new Dictionary<int, CardSFXPair>();
    
    public void EnterBattle(Beast beast)
    {
        foreach (var item in PlayerParty.inst.party)
        {
            deckDict.Add(item,item.deck);
           // deckDict[item].cards = new List<Card>(item.deck.cards);
        }
        
        foreach (var item in hand)
        {Destroy(item.gameObject);}
     
        SwitchBeast(beast);
        currentDeck.Shuffle();
        CreateCardBehaviour(DrawCard());
        CreateCardBehaviour( DrawCard());
        CreateCardBehaviour(DrawCard());
        MakeHandInteractable();
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

    public void CreateVapourCard(Card card)
    {
        if(hand.Count < 6) //does not work if hand is full
        {
            EventManager.inst.onPlayerVapourCardCreation.Invoke();
            CreateCardBehaviour(card,CardState.VAPOUR);
        }

    }



    public void AddManifestedCardToDeck(Card c){
        if(hand.Count < 7)
        {
            CreateCardBehaviour(c,CardState.MANIFESTED);
        }
    }

    public void ActivateHand()
    {
        handDown = false;
        holder.DOAnchorPosY(0,.2f).OnComplete(()=>
        {
           EndTurnButton.inst.Reactivate();
        });
         MakeHandInteractable();
    }

    public void MakeHandInteractable()
    {
        foreach(CardBehaviour card in hand)
        {  card.EnableInteractable();}
    }
    
    public void DeactivateHand()
    {EndTurnButton.inst.Deactivate();
        handDown = true;
        holder.DOAnchorPosY(downHodlerPos,.2f);
        foreach(CardBehaviour card in hand)
        {card.DisableInteractable();}
    }

    public void HideHand(){
        handDown = true;
        holder.DOAnchorPosY(-550,.2f);
        foreach(CardBehaviour card in hand)
        {card.DisableInteractable();}
    }

    public void AddPredeterminedDraw(Card card, DrawCardAtSpecifiedTurn dcst)
    {
        int i = BattleManager.inst.turn + dcst.inHowManyTurns;
        CardSFXPair csfx = new CardSFXPair();
        csfx.card = card;
        csfx.sfx = dcst.sfxSummon;
   
        if(!predetermindedDraws.ContainsKey(i))
        {
            predetermindedDraws.Add(i,csfx);
        }
        else
        {
            predetermindedDraws[i] = csfx;
        }
    }

    public bool currentTurnDrawIsPredeterminded()
    {return predetermindedDraws.ContainsKey(BattleManager.inst.turn);}

    public Card DrawPredeterminedCard(){
        Card c = predetermindedDraws[BattleManager.inst.turn].card;
        AudioManager.inst.GetSoundEffect().Play(predetermindedDraws[BattleManager.inst.turn].sfx);
        predetermindedDraws.Remove(BattleManager.inst.turn);
        return c;
    }

    public Card DrawCardOfSpecificTrait(BeastClass beastClass)
    {
        if(!BattleManager.inst.inBattle)
        { return null; }
        if(currentDeck.cards.Count <= 0)
        { currentDeck.ResetDiscardPile(); }
        
        if(hand.Count < 7)
        { return CardFunctions.DrawCardOfSpecificTrait(currentDeck,beastClass); }
        else
        { Debug.Log("Hand is full!"); 
        return null;}
    }

   

    public Card DrawCard()
    {
        if(!BattleManager.inst.inBattle){
            return null;
        }
        if(currentDeck.cards.Count <= 0)
        { currentDeck.ResetDiscardPile(); }
        
        if(hand.Count < 7)
        { 
            if(hand.Count == 6)
            {
                if(!playableCardInHand())
                {
                    if(playableCardInDeck()){
                           Debug.Log("Playable card found in main deck");
                    return CardFunctions.DrawPlayableCard(currentDeck);
                       
                    }
                    else if(playableCardInDiscard())
                    {
                        Debug.Log("Playable card found in discard");
                        currentDeck.ResetDiscardPile();
                           return CardFunctions.DrawPlayableCard(currentDeck);
                      
                        
                    }
                    else
                    {
                        Debug.Log("No playable cards anywhere XD");
                    }
                  
                    return null;
                }
            }
           
           
           // Card c = 
            return CardFunctions.DrawRandomCard(currentDeck);
            // CreateCardBehaviour(c); 
            // MakeHandInteractable();
            
            
        }
        else
        { Debug.Log("Hand is full!"); 
         return null;}
    }

    

    public bool playableCardInHand(){
        bool b = false;
        foreach (var card in hand)
        {
            if(!card.card.unplayable)
            {
                b = true;
            }
        }
        return b;
    }

    public bool playableCardInDeck()
    {
        bool b = false;
        foreach (var card in currentDeck.cards)
        {
            if(!card.unplayable)
            {
                b = true;
            }
        }
        return b;
    }

    public bool playableCardInDiscard()
    {
        bool b = false;
        foreach (var card in currentDeck.discard)
        {
            if(!card.unplayable)
            {
                b = true;
            }
        }
        return b;
    }



   
    public void CheckForHandFuckery()
    {
        List<CardBehaviour> fuckery = new List<CardBehaviour>();
        foreach (var item in hand)
        {
            if(item.state == CardState.VAPOUR){
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

 

    public void CreateCardBehaviour(Card card,CardState state = CardState.NORMAL)
    {
        if(card != null)
        {
            EventManager.inst.onPlayerDrawCard.Invoke();
            CardBehaviour newCard = Instantiate(prefab,holder);
            newCard.Init(card,state);
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
            CardManager.inst.DeactivateHand();
            behaviour.gameObject.SetActive(false);
            if(usedCard.playVFXAfterDelay)
            {
                if(usedCard.vfxSetUp!=string.Empty)
                {BattleEffectManager.inst.Play(usedCard.vfxSetUp);}
            }
            
            Dictionary<Entity,HitData> dict = new Dictionary<Entity, HitData>();
            bool atleastOneHit = false;
         
            foreach (var effect in usedCard.effects)
            { 
                EffectArgs args = new EffectArgs(PlayerParty.inst.activeBeast,null,  null,null,-99,"");
               
                foreach (var target in effect.AffectedEntities(args))
                {
                    if(target != null)
                    {
                        if(!dict.ContainsKey(target))
                        {
                            HitData data = new HitData();
                            data.entity = target;
                            dict.Add(target,data);
                            if(effect.offensive)
                            { 
                                if(target.stunned|usedCard.unDodgeable)
                                { data.dodged = false;}
                                else{ data.dodged = Maths.PercentCalculator(target.dodge);}
                                if(!data.dodged)
                                {atleastOneHit = true;}
                            }
                            else
                            {
                                data.dodged = false;
                                atleastOneHit = true;
                            }
                        }
                    }
                    else
                    {dict[target].effects.Add(effect);}
                   
                }
            }

            foreach (var effect in usedCard.effects)
            { 
                foreach (var item in dict)
                {               
                    EffectArgs args = new EffectArgs(PlayerParty.inst.activeBeast,null,  null,null,-99,"");
                 if(effect.AffectedEntities(args).Contains(item.Value.entity))
                 {item.Value.effects.Add(effect);}    
                    
                }
            }

            if(atleastOneHit)
            {AudioManager.inst.GetSoundEffect().Play(usedCard.soundEffect);}
            else
            {AudioManager.inst.GetSoundEffect().Play(usedCard.missSound);}
      
            yield return new WaitForSeconds(usedCard.castDelay);


            BattleEffectManager.inst.Play(usedCard.vfx);

            if(behaviour.state == CardState.NORMAL)
            {
                if(!usedCard.destroyOnCast)
                {currentDeck.discard.Add(usedCard);}
            }

            if(usedCard.playVFXAfterDelay)
            {BattleEffectManager.inst.Play(usedCard.vfx);}

            BattleTicker.inst.Type(usedCard.cardName);
            ManaManager.inst.Spend(usedCard.manaCost);

            
               
            RemoveFromHand(behaviour);
            CardStackBehaviour stackBehaviour =  CardStack.inst.CreateActionStack(usedCard,PlayerParty.inst.activeBeast);
   
            foreach (var targets in dict)
            {
               
                foreach (var effect in targets.Value.effects)
                { 
                    EffectArgs args = new EffectArgs(PlayerParty.inst.activeBeast,targets.Value.entity, usedCard,stackBehaviour,
                    BattleManager.inst.playerRecord[BattleManager.inst.turn].cardsPlayedThisTurn.Count-1,usedCard.cardName);
                    
                    if(effect.offensive)
                    {
                        if(targets.Value.dodged) //if the target dodged
                        {targets.Value.entity.animatedInstance.Dodge();}
                        else
                        {effect.Use(args);}
                    }
                    else
                    { effect.Use(args); }
                }
            }

           
            BattleManager.inst. StartCoroutine(piss());
            IEnumerator piss()
            {
                while(BattleManager.inst.FUCKOFF)
                {yield return null;}
                BattleManager.inst.CheckForStatusEffectBeforeAllowingCards();
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
        foreach (var item in deckDict)
        {
           foreach (var card in item.Value.discard)
           {
                item.Value.cards.Add(card);
           } 
           item.Value.discard.Clear();
        }
        deckDict.Clear();
        
        hand.Clear();
        currentBeast = null;
        currentDeck = null;
        DeactivateHand();

      
    }
}