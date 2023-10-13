using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck
{
    public List<Card> cards = new List<Card>();
    public List<Card> discard = new List<Card>();


    public void ResetDiscardPile(){

        if(cards.Count <= 0)
        {
            cards = new List<Card>(discard);
            discard.Clear();
            Debug.Log("Resetting discard pile");
        }
    }
}