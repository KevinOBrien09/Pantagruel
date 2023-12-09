using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class LocationSaveData{
    public string mainLocation;
    public int subLocation;
}

public class LocationManager : Singleton<LocationManager>
{
    public MainLocation gameStart;
    public MainLocation currentMainLoc;
    public Location currentSubLocation;
    public Image blackFade;
    public Dictionary<Biome,List<BeastScriptableObject>> dict = new Dictionary<Biome, List<BeastScriptableObject>>();
    public List<MainLocation> allMainLocs = new List<MainLocation>();
    Dictionary<string,MainLocation> mainLocDict = new Dictionary<string, MainLocation>();
    public GameObject currentSubLocationInstance;
    public bool EDITINGINSTANCE;

    protected  override void Awake()
    {
        foreach (var item in allMainLocs)
        {mainLocDict.Add(item.Id,item);}
        base.Awake();
  
    }

    public void ChangeMainLocation(MainLocation mainLocation)
    {
        currentMainLoc = mainLocation;
        ChangeSubLocation(mainLocation.subLocations[0],mainLocation.playerPos,mainLocation.playerRot);

        UpperLeftPanel.inst.ChangeLocationText(currentMainLoc); 
    }

    public void ChangeSubLocationWithFade(Location newLocation,Vector3 p,Vector3 r)
    {
        OverworldMovement.canMove = false;
        blackFade.DOFade(1,.25f).OnComplete(()=>
        {
            ChangeSubLocation(newLocation,p,r);
            StartCoroutine(q());
            IEnumerator q()
            {
                yield return new WaitForSeconds(.2f);
                blackFade.DOFade(0,.25f).OnComplete(()=>{  OverworldMovement.canMove =  true;});
            }
        });
    }

    void ChangeSubLocation(Location newLocation,Vector3 p,Vector3 r){
        currentSubLocation = newLocation;
        BattleTicker.inst.Type(currentSubLocation.locationName);
        PlayerManager.inst.movement.detectEncouters = currentSubLocation.detectEncouters;
        PlayerManager.inst.movement.InitPosRot(p,r);
        PlayerManager.inst.movement.rotate.InitRotation(NESW.inst.GetDirection(PlayerManager.inst.movement.rotate.transform));
        if(!EDITINGINSTANCE)
        {
            if(currentSubLocationInstance != null)
            {Destroy(currentSubLocationInstance);}
            currentSubLocationInstance = Instantiate(currentSubLocation.prefab,Vector3.zero,Quaternion.identity);
        }
        Interactor.inst.RenableInteraction();
        OrgniseDict();
    }

    public LocationSaveData Save(){
        LocationSaveData saveData = new LocationSaveData();
        saveData.mainLocation = currentMainLoc.Id;
        saveData.subLocation = currentMainLoc.subLocations.IndexOf(currentSubLocation);
        return saveData;
    }

    public void Load(LocationSaveData locationSaveData){
        currentMainLoc = mainLocDict[locationSaveData.mainLocation];
        currentSubLocation = currentMainLoc.subLocations[locationSaveData.subLocation];
        currentSubLocationInstance = Instantiate(currentSubLocation.prefab,Vector3.zero,Quaternion.identity);
        Interactor.inst.RenableInteraction();
        UpperLeftPanel.inst.ChangeLocationText(currentMainLoc); 
        BattleTicker.inst.Type(currentSubLocation.locationName);
        PlayerManager.inst.movement.detectEncouters = currentSubLocation.detectEncouters;
        PlayerManager.inst.movement.rotate.InitRotation(NESW.inst.GetDirection(PlayerManager.inst.movement.rotate.transform));
        OrgniseDict();
    }

    void OrgniseDict()
    {
      
        dict.Clear();
        foreach (var item in currentSubLocation.beastsInLocation)
        {dict.Add(item.biome,item.scriptableObject);}
    }


    public BeastScriptableObject GetEncounter(Biome biome)
    {
        return dict[biome][Random.Range(0,dict[biome].Count)];
    }


}