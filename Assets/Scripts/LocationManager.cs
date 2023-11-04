using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : Singleton<LocationManager>
{
    public Location testing;
    public Location currentLocation;
    public Dictionary<Biome,List<BeastScriptableObject>> dict = new Dictionary<Biome, List<BeastScriptableObject>>();

    protected  override void Awake()
    {
     base.Awake();
        OrgniseDict(testing);
    }

    void OrgniseDict(Location newLocation)
    {
        currentLocation = newLocation;
        dict.Clear();
        foreach (var item in newLocation.beastsInLocation)
        {dict.Add(item.biome,item.scriptableObject);}
    }


    public BeastScriptableObject GetEncounter(Biome biome)
    {
        return dict[biome][Random.Range(0,dict[biome].Count)];
    }


}