using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Location")]
public class Location : ScriptableObject
{
 
   
    public string locationName;
    public GameObject prefab;
    public SoundData bgMusic;
    public SoundData footStep;
    public bool detectEncouters;
    public List<BeastScriptableObject> beastsInLocation = new List<BeastScriptableObject>();
    public List<Card> cardRewards = new List<Card>();
    public Vector2 goldRewardRange;
    public Vector2 levelRange; //if mobs scale with player throughout game move this elsewhere.
    public bool isCutScene;
    public Dialog dialogToLoadOnEnter;
}
