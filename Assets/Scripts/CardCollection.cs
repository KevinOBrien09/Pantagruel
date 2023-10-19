using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollection : Singleton<CardCollection>
{
    public List<Card> ownedCards = new List<Card>();
    public List<Card> knownCards = new List<Card>();

    public void RemoveCard(Card card){
        ownedCards.Remove(card);
    }

     public void AddCard(Card card){
        ownedCards.Add(card);
    }

    public List<string> Save(){
        List<string> data = new List<string>();

        foreach (var item in ownedCards)
        {
            data.Add(item.Id);
            
        }

        return data;
    }

    public void Load(List<string> saveData){
        ownedCards.Clear();

        foreach (var id in saveData)
        {
            if(BeastBank.inst.cardDict.ContainsKey(id))
            {ownedCards.Add(BeastBank.inst.cardDict[id]);}

        }
    }

    public bool ElementIsInCollection(Element e)
    {
        foreach (var item in ownedCards)
        {
            if(item.element == e){
                return true;
            }
            
        }
        return false;

    }

     public bool ClassIsInCollection(BeastClass b)
    {
        foreach (var item in ownedCards)
        {
            if(item.beastClass == b){
                return true;
            }
            
        }
        return false;

    }
}