using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CardFunctions
{
    public static Card DrawRandomCard(Deck deck)
    {
        if(deck.cards.Count > 0)
        {
           
            Card card = deck.cards[0];
            deck.cards.Remove(card);
            return card;
        }
        else
        {
            Debug.Log("Deck is empty");
            return null;
        }
    }

    public static Card DrawCardOfSpecificTrait(Deck deck,BeastClass beastClass)
    {
        if(deck.deckContainsCardOfTrait(beastClass)){
            if(deck.cards.Count > 0)
            {
                foreach (var item in deck.cards)
                {
                    if(item.beastClass == beastClass){
                        Card card = item;
                        deck.cards.Remove(card);
                        return card;
                    }
                }
            
               
            }
            else
            {
                Debug.Log("Deck is empty");
                return null;
            }
        }
        return null;
     
    }

    public static Card DrawPlayableCard(Deck deck)
    {
        if(deck.cards.Count > 0)
        {
            Card c = null;
            foreach (var item in deck.cards)
            {
                Debug.Log(item.cardName);
                if(!item.unplayable){
                    c = item; 
              //      break;
                }
            }
            if(c != null){
                deck.cards.Remove(c);
            }
            else{
                Debug.Log("QWERTYASDF");
            }
           
      
            return c;
        }
        else
        {
            Debug.Log("Deck is empty");
            return null;
        }
    }

    public static Card DrawCardOfSpecificCost(List<int> cost,Deck deck)
    {
        List<Card> validCards = new List<Card>();
        foreach (var card in deck.cards)
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
            DrawCardOfSpecificCost(newCostList,deck);
            return null;
        }
        else
        {
            Card card1 = validCards[Random.Range(0,validCards.Count)];
            deck.cards.Remove(card1);
            return card1;
        }
    }

    public static bool canCast(Card c, bool isPlayer)
    {
        bool hasMana = ManaManager.inst.currentMana >= c.manaCost;
      
        if(hasMana && oneEffectIsViable(c.effects,isPlayer))
        {return true;}
        else
        {return false;}

       
    }

    public static bool oneEffectIsViable(List<Effect> effects,bool isPlayer)
    {
        bool b = false;
        foreach (var item in effects)
        {
            if(item.canUse(isPlayer))
            { b = true;}
            else
            {Debug.Log(item.name +" is not viable");}
        }
        return b;

        }

}