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
    public string vfx;
    public float castDelay = .2f;
    public List<Effect> effects = new List<Effect>();
    public SoundData soundEffect;

    [TextArea] public string desc;
    public int deckCost;
  
}