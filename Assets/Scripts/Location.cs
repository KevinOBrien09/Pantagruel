using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Location")]
public class Location : ScriptableObject
{
    [System.Serializable]
    public class BeastSOBiomePair
    {
        public List<BeastScriptableObject> scriptableObject = new List<BeastScriptableObject>();
        public Biome biome;
    }
    public string locationName;
    public List<BeastSOBiomePair> beastsInLocation = new List<BeastSOBiomePair>();
    public List<Card> cardRewards = new List<Card>();
    public Vector2 goldRewardRange;
    public Vector2 levelRange; //if mobs scale with player throughout game move this elsewhere.
}
