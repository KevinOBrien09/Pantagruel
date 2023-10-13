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
           
            Card card = deck.cards[Random.Range(0,deck.cards.Count)];
            deck.cards.Remove(card);
            return card;
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
}