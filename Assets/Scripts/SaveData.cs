using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class SaveData
{
   // public BeastSaveData activeBeast;
    public List<BeastSaveData> party = new List<BeastSaveData>();
    public List<BeastSaveData> backBenched = new List<BeastSaveData>();
    public PlayerSaveData playerSaveData;
    // public int scene;
    // public int gold;
    // public Date date;
    // public EXPSave exp;
    // public List<UnitSaveData> party = new List<UnitSaveData>();
    // public List<InventorySaveData> inventory = new List<InventorySaveData>();
    // public List<UnitSaveData> enemies = new List<UnitSaveData>();
    // public CameraFollow.CameraSaveData cameraSaveData;
    // public SceneState sceneState;
}

