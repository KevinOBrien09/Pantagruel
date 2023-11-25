using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck
{
    public List<Card> cards = new List<Card>();
    public List<Card> discard = new List<Card>();

    public bool AddCardToDeck(Card card)
    {
        if(canFitCard(card.deckCost))
        {
            cards.Add(card);
            return true;
        }
        else
        {return false;}
    }

    public void RemoveCardFromDeck(Card card)
    { cards.Remove(card); }

    public void DestroyCard(Card card)
    {
        cards.Remove(card);
        discard.Remove(card);

    }

    bool canFitCard(int cardCost)
    {
        if(TotalDeckCost() + cardCost <= 100)
        {return true;}
        else
        {return false;}
    }
    
    public void ResetDiscardPile(){

        
        foreach (var item in discard)
        {
            cards.Add(item);
        }
    
        discard.Clear();
        Debug.Log("Resetting discard pile");
    
    }

    public int TotalDeckCost()
    {
        int i = 0;
        foreach (var item in cards)
        {i+=  item.deckCost;}
        return i;
    }
}