using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerManager : Singleton<PlayerManager>
{
 
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
        (BeastSaveData a,List<BeastSaveData> b)  partySave = PlayerParty.inst.Save();
        mainData.party = partySave.b;
        mainData.playerSaveData = movement.Save();
        return mainData;
    }

    public void Load()
    {
        SaveData data = SaveLoad.Load(5);
        PlayerParty.inst.Load(data);
        movement.Load(data.playerSaveData);
    }
}