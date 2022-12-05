using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Room", menuName ="Dungeon/Room")]
public class DungeonRoom : ScriptableObject
{
    public string prompt;
    public List<ExplorationAction> options = new List<ExplorationAction>();
    public GameObject roomPrefab;
}

[System.Serializable]
public class ExplorationAction
{
    public List<string> validResponse = new List<string>();
    public string logString;
    public int encounterChanceWhenGoingToNextRoom;
    public DungeonRoom nextRoom;
    public ExplorationResult result;

    public void ForceToLower()
    {
        foreach (var item in validResponse)
        {item.ToLower();}
    }
}
