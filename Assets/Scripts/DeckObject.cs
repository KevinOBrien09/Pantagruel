using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Deck")]
public class DeckObject : ScriptableObject
{
    public Deck deck;
    public List<CardCombo> combos = new List<CardCombo>();

}