using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerParty party;
    public OverworldMovement movement;
    public bool load;

    void Start()
    {
        if(load)
        {
            Load();
        }
    }
    
    public SaveData Save()
    {
        SaveData mainData = new SaveData();
        (BeastSaveData a,List<BeastSaveData> b)  partySave = PlayerManager.inst.party.Save();
        mainData.party = partySave.b;
        mainData.activeBeast = partySave.a;

        mainData.playerSaveData = movement.Save();
  
        return mainData;
    }

    public void Load()
    {
        SaveData data = SaveLoad.Load(5);
        party.Load(data);
        movement.Load(data.playerSaveData);
    }
}