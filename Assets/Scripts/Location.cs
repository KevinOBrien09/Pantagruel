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
    public List<BeastSOBiomePair> beastsInLocation = new List<BeastSOBiomePair>();
}
