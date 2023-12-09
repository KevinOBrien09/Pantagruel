using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class SaveData
{
   // public BeastSaveData activeBeast;
    public List<BeastSaveData> party = new List<BeastSaveData>();
    public List<BeastSaveData> backBenched = new List<BeastSaveData>();
    public List<string> cardCollection = new List<string>();
    public PlayerSaveData playerSaveData;
    public LocationSaveData locationSaveData;
   
}

