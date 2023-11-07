using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using System;

public class CardManager:Singleton<CardManager>                   
{
    public CardBehaviour prefab;
    public RectTransform holder;
    public List<CardBehaviour> hand = new List<CardBehaviour>();
    public List<Card> cardsUsedThisTurn = new List<Card>();
    
    float holderOgPos;
    public float downHodlerPos;
    public Deck currentDeck;
    public Beast currentBeast;
    public SoundData drawCard;
    Dictionary<Beast,Deck> deckDict = new Dictionary<Beast, Deck>();
    public bool handDown;
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

    public bool canCast(Card card,bool isPlayer)
    {
        bool hasMana = ManaManager.inst.currentMana >= card.manaCost;
      
        if(hasMana && oneEffectIsViable())
        {return true;}
        else
        {return false;}

        bool oneEffectIsViable(){

            bool b = false;
            foreach (var item in card.effects)
            {
                if(item.canUse(isPlayer))
                { b = true;}
                else{
                    Debug.Log(item.name +" is not viable");
              
                }
            }

            return b;

        }
    }

    public void NewTurn()
    {
        cardsUsedThisTurn.Clear();
    }

    public void Use(Card usedCard,CardBehaviour behaviour)
    {
        StartCoroutine(q());
        cardsUsedThisTurn.Add(usedCard);
  
        EventManager.inst.onPlayerCastCard.Invoke();
        IEnumerator q()
        {
            if(usedCard.castDelay != 0){
  CardManager.inst.DeactivateHand();
            }
          
            if(!usedCard.playVFXAfterDelay){
            BattleEffectManager.inst.Play(usedCard.vfx);
            }

            if(usedCard.playVFXAfterDelay){
                if(usedCard.vfxSetUp!=string.Empty){
                BattleEffectManager.inst.Play(usedCard.vfxSetUp);    

                }
            }
            
            
            if(usedCard.soundEffect.audioClip != null)
            {AudioManager.inst.GetSoundEffect().Play(usedCard.soundEffect); }
            behaviour.gameObject.SetActive(false);
            yield return new WaitForSeconds(usedCard.castDelay);
            if(!behaviour.isVapour){
                currentDeck.discard.Add(usedCard);
            }
            if(usedCard.playVFXAfterDelay){
            BattleEffectManager.inst.Play(usedCard.vfx);
            }
            BattleTicker.inst.Type(usedCard.cardName);
             StartCoroutine(piss());
            ManaManager.inst.Spend(usedCard.manaCost);
            bool cardContainsDrawEffect = usedCard.effects.OfType<DrawCardEffect>().Any();
            if(cardContainsDrawEffect)
            {RemoveFromHand(behaviour);}
            else
            {RemoveFromHand(behaviour);}
            foreach (var effect in usedCard.effects)
            { 
                EffectArgs args = new EffectArgs(PlayerParty.inst.activeBeast,BattleManager.inst.enemyTarget,true,usedCard);
                effect.Use(args); 
            }


            if(usedCard.castDelay != 0){
                CardManager.inst.ActivateHand();
            }
            IEnumerator piss(){
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
        DestroyHand();
        deckDict.Clear();
        cardsUsedThisTurn.Clear();
        hand.Clear();
        currentBeast = null;
        currentDeck = null;
        DeactivateHand();

      
    }
}