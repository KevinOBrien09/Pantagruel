 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card")]
public class Card :GUIDScriptableObject
{
    public string cardName;
    public Sprite picture;
    public int manaCost;
    public string vfxSetUp, vfx;
    public float castDelay = .2f;
    public List<Effect> effects = new List<Effect>();
    public bool playVFXAfterDelay;
    public SoundData soundEffect,missSound;
    public Element element;
    public BeastClass beastClass;
    [TextArea(5,5)] public string desc;
     [TextArea] public string flavourText;
    public int deckCost;
    public bool unplayable,cannotBeDiscard,destroyOnCast,unDodgeable;
  
}
