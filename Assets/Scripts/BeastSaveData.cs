using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeastSaveData 
{
    public int beastiaryID;
    public float currentHealth;
    public List<string> deckIDs = new List<string>(); 
    public EXPSave exp;
    public List<StatusEffects> statusEffects = new List<StatusEffects>();
    public Sin sin;
    //public LocationSer
    
}
