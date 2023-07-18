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
    public ParticleSystem useCardEffect;
    public List<int> manaCostsThatCanBeDrawn = new List<int>();
    float holderOgPos;
    public float downHodlerPos;

    public List<Card> discardPile = new List<Card>();
    public void EnterBattle(Beast beast)
    {
        foreach (var item in hand)
        {Destroy(item.gameObject);}
        manaCostsThatCanBeDrawn.Add(1);
        
        DrawCardOfSpecificCost(manaCostsThatCanBeDrawn,beast);
        ActivateHand();
    }

    public void ActivateHand()
    {
        
        holder.DOAnchorPosY(0,.2f).OnComplete(()=>
        {
            MakeHandInteractable();
        });
    }

    public void MakeHandInteractable(){
         foreach(CardBehaviour card in hand)
        {  card.EnableInteractable();}
    }
    
    public void DeactivateHand()
    {
        holder.DOAnchorPosY(downHodlerPos,.2f);
        
        // .OnComplete(()=>
        // {
            foreach(CardBehaviour card in hand)
            {card.DisableInteractable();}
        //});
    }

    public void DrawRandomCard(Beast b)
    {
        if(hand.Count < 7){
            Card card = b.deck.cards[Random.Range(0,b.deck.cards.Count)];
            CardBehaviour newCard = Instantiate(prefab,holder);
            newCard.Init(card);
            hand.Add(newCard);
        }
        else{
            Debug.Log("Hand is full!");
        }
       
    }

    public void DrawCardOfSpecificCost(List<int> cost,Beast b)
    {
        if(hand.Count < 7){

            List<Card> validCards = new List<Card>();
            foreach (var card in b.deck.cards)
            {   
                if(cost.Contains(card.manaCost))
                {validCards.Add(card);}
            }
            
            if(validCards.Count == 0)
            {
                List<int> newCostList = new List<int>(cost);
                int i = newCostList.Max();
                i++;
                newCostList.Add(i);
                DrawCardOfSpecificCost(newCostList,b);
            }
            else
            {
                Card card1 = validCards[Random.Range(0,validCards.Count)];
                CardBehaviour newCard = Instantiate(prefab,holder);
                newCard.Init(card1);
                hand.Add(newCard);
            }
        }
        else{
            Debug.Log("Hand is full!");
        }
       
    }

    public bool canCast(Card card)
    {
        if(ManaManager.inst.currentMana >= card.manaCost)
        {return true;}
        else
        {return false;}
    }

    public void Use(Card usedCard,CardBehaviour behaviour)
    {
        
        discardPile.Add(usedCard);
        CardStack.inst.CreateActionStack(usedCard,PlayerManager.inst.party.activeBeast);
        ManaManager.inst.Spend(usedCard.manaCost);
        foreach (var effect in usedCard.effects)
        {
            effect.Use(PlayerManager.inst.party.activeBeast,RivalBeastManager.inst.currentBeast);
        }
        hand.Remove(behaviour);
        Destroy(behaviour.gameObject);
    }
}