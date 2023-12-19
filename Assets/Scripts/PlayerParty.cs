using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerParty : Singleton<PlayerParty>
{
    public List<Beast> party = new List<Beast>(); 
    public Beast activeBeast;
    public Beast beastPrefab;
    
    void Start()
    {
        if(!PlayerManager.inst.load)
        { 
            LocationManager.inst.   ChangeMainLocation( LocationManager.inst.   gameStart);
            List<Beast> p = new List<Beast>(party);
            List<BeastSaveData> saveDatas = new List<BeastSaveData>();
            foreach (var item in party)
            {Destroy(item.gameObject);}
            party.Clear();
            foreach (var item in p)
            {
                EXP e = new EXP();
                e.PsudeoLevel((int)Random.Range(LocationManager.inst.currentSubLocation.levelRange.x,LocationManager.inst.currentSubLocation.levelRange.y),item);
                BeastSaveData bsd = item.PsudeoSave(item.scriptableObject);
                EXPSave save = new EXPSave();
                
                save.currentEXP = e.currentExp;
                save.level = e.level;
                save.targetEXP = e.targetExp;
                bsd.exp = save;
               
               item.FirstHealthInit(e);
                item.ownership = EntityOwnership.PLAYER;
                bsd.currentHealth = item.stats().maxHealth;
                saveDatas.Add(bsd);
                if(party.Count == 0){
                BottomCornerBeastDisplayer.inst.NoBeast();   
                }
            }
            Load(saveDatas);
         
      
        }
    }

    public void RemoveStatusEffectsEndOfCombat()
    {
        foreach (var item in party)
        {
            item.currentShield = 0;
            item.shields.Clear();
            foreach (var b in item.currentHealthBars)
            {
                b.UpdateFill();
                b.UpdateText();
            }
            item.statusEffectHandler.WipeAtEndOfCombat();
        }
    }


    public void Load( List<BeastSaveData>  data)
    {
        party = new List<Beast>();
        foreach (Transform item in transform)
        { Destroy(item.gameObject); }
        
        for (int i = 0; i < data.Count; i++)  
        {
            Beast b = Instantiate(beastPrefab,transform);
            b.Init(data[i]);
            AddNewBeast(b);
            b.exp = new EXP();
            b.exp.Load(data[i].exp);
            b.exp.beast = b;
            b.sin = data[i].sin;
            b.ownership = EntityOwnership.PLAYER;
           
        }
        
        if(party.Count == 0)
        {
            BottomCornerBeastDisplayer.inst.NoBeast();   
        }
        else
        {
            BottomCornerBeastDisplayer.inst.YesBeast();   
            activeBeast = party[0];
            BottomCornerBeastDisplayer.inst.CreateAnimatedInstances(party);
            ApplyLoadedInfo();
            StartCoroutine(q());
        }
        
        IEnumerator q()
        {
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < data.Count; i++)  
            {party[i].statusEffectHandler.Load(data[i].statusEffects);}
        }
       
    }

    public void ChangePartyOrder(Beast newActiveBeast){
       
        party.RemoveAt(party.IndexOf(newActiveBeast));
        party.Insert(0,newActiveBeast);
        activeBeast = newActiveBeast;

    }
    
    void ApplyLoadedInfo()
    {
        
          PassiveManager.inst.OrginizePassiveActivity();
        BottomCornerBeastDisplayer.inst.ChangeActiveBeast(activeBeast,false);
      
        
    }

    public void AddNewBeast(Beast b)
    {
        b.ownership = EntityOwnership.PLAYER;
        if(b.scriptableObject.beastData.passive != null)
        {
            PassiveManager.inst.AddNewBeast(b);
            
    
        }
        else{
            Debug.Log(b.name + " passive not set up");
        }
        
        party.Add(b);
    }

    #if UNITY_EDITOR
    [MenuItem("Func/Save")]
    static void DebugSave(){
        SaveLoad.Save(5);
    }

    [MenuItem("Func/Load")]
    static void DebugLoad()
    {
      Debug.LogWarning("Not Implemented");
    }

  #endif

    public (BeastSaveData a ,List<BeastSaveData> b) Save()
    {
        BeastSaveData a = ToSave(activeBeast);
        List<BeastSaveData> l = new List<BeastSaveData>();
        foreach (var item in party)
        {
            l.Add(ToSave(item));
        }

        BeastSaveData ToSave(Beast b)
        {
            if(b !=null)
            {
                BeastSaveData saveData = new BeastSaveData();
                saveData.beastiaryID = b.scriptableObject.beastData.bestiaryID;
                saveData.currentHealth = b.currentHealth;
                List<string> cardIDs = new List<string>();
                foreach (var card in b.deck.cards)
                {
                    if(card.Id != ParasiteEffect.parasiteCard){
         cardIDs.Add(card.Id); 
                    }
            
                }
                saveData.deckIDs = cardIDs;
                saveData.exp = b.exp.Save();
                saveData.statusEffects = b.statusEffectHandler.Save();
                saveData.sin = b.sin;
                return saveData;
            }
            else   
            {
                Debug.LogWarning("Beast is null.");
                return null;
            }
        }
        return (a,l);
    }
    
}
