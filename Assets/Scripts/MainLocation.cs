using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MainLocation")]
public class MainLocation : GUIDScriptableObject
{
    public string locName;
    public SoundData bgMusic;
    public List<Location> subLocations = new List<Location>();
    [Header("Where player spawns for first time in starting area ([0] in list)")]
    public Vector3 playerPos;
    public Vector3 playerRot; 
}
