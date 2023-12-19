using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildEncounterManager : Singleton<WildEncounterManager>
{

    public List<BeastScriptableObject> GetEncounter()
    {
        List<BeastScriptableObject> beasts = new List<BeastScriptableObject>();
        BeastScriptableObject encounteredBeast = LocationManager.inst.GetEncounter();
        beasts.Add(encounteredBeast);
        return beasts;
    }

}