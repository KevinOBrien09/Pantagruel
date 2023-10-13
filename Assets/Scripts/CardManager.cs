using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class CardManager:Singleton<CardManager>                   
{
    public CardBehaviour prefab;
    public RectTransform holder;
    public List<CardBehaviour> hand = new List<CardBehaviour>();
    public List<Card> cardsUsedThisTurn = new List<Card>();
    public List<int> manaCostsThatCanBeDrawn = new List<int>();
    float holderOgPos;
    public float downHodlerPos;
    public Deck currentDeck;
    public Beast currentBeast;
    public SoundData drawCard;
    Dictionary<Beast,Deck> deckDict = new Dictionary<Beast, Deck>();

    public void EnterBattle(Beast beast)
    {
        foreach (var item in PlayerParty.inst.party)
        {
            deckDict.Add(item,new Deck());
            deckDict[item].cards = new List<Card>(item.deck.cards);
        }
        
        foreach (var item in hand)
        {Destroy(item.gameObject);}
        manaCostsThatCanBeDrawn.Add(1);
        SwitchBeast(beast);
        DrawCardOfSpecificCost(manaCostsThatCanBeDrawn);
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
        holder.DOAnchorPosY(downHodlerPos,.2f);
        foreach(CardBehaviour card in hand)
        {card.DisableInteractable();}
    }

    public void DrawRandomCard(Beast b)
    {
        if(currentDeck.cards.Count <= 0)
        { currentDeck.ResetDiscardPile(); }
        
        if(hand.Count < 7)
        { CreateCardBehaviour(CardFunctions.DrawRandomCard(currentDeck)); }
        else
        { Debug.Log("Hand is full!"); }
    }

    public void DrawCardOfSpecificCost(List<int> cost)
    {
        if(currentDeck.cards.Count <= 0)
        {currentDeck. ResetDiscardPile();}

        if(hand.Count < 7)
        {CreateCardBehaviour(CardFunctions.DrawCardOfSpecificCost(cost,currentDeck));}
        else
        {Debug.Log("Hand is full!");}
       
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

    public bool canCast(Card card)
    {
        if(ManaManager.inst.currentMana >= card.manaCost)
        {return true;}
        else
        {return false;}
    }

    public void NewTurn()
    {
        cardsUsedThisTurn.Clear();
    }

    public void Use(Card usedCard,CardBehaviour behaviour)
    {
        StartCoroutine(q());
        cardsUsedThisTurn.Add(usedCard);
        BeastSwapper.inst.DeactivateButton();
        EventManager.inst.onPlayerCastCard.Invoke();
        IEnumerator q()
        {
            BattleEffectManager.inst.Play(usedCard.vfx);
            
            if(usedCard.soundEffect.audioClip != null)
            {AudioManager.inst.GetSoundEffect().Play(usedCard.soundEffect); }
         
            yield return new WaitForSeconds(usedCard.castDelay);
            if(!behaviour.isVapour){
                currentDeck.discard.Add(usedCard);
            }
          
            BattleTicker.inst.Type(usedCard.cardName);
            ManaManager.inst.Spend(usedCard.manaCost);
            foreach (var effect in usedCard.effects)
            { effect.Use(PlayerParty.inst.activeBeast,RivalBeastManager.inst.activeBeast); }
            RemoveFromHand(behaviour);
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