using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollection : Singleton<CardCollection>
{
    public List<Card> ownedCards = new List<Card>();
    public List<Card> knownCards = new List<Card>();
}