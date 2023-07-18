using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card")]
public class Card :ScriptableObject
{
    public int cardID;
    public string cardName;
    public Sprite picture;
    public int manaCost;
    public List<Effect> effects = new List<Effect>();

    [TextArea] public string desc;
    public int deckCost;
  
}