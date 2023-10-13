using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item")]
public class Item :GUIDScriptableObject 
{
   
    public int itemID;
    public string itemName;
    public Sprite picture;
    public bool castable;
    public bool battleCastableOnly;
    public bool only1PerTurn;
    public List<Effect> effects = new List<Effect>();
    public SoundData soundEffect;
    public int goldCost;
    [TextArea] public string desc;
  
  
}