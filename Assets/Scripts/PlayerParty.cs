using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerParty : Singleton<PlayerParty>
{
    public List<Beast> party = new List<Beast>(); //does not contain activeBeast
    
    public Beast activeBeast;
    public Beast beastPrefab;
    
    void Start()
    {
        if(!PlayerManager.inst.load)
        { 
            foreach (var item in party)
            {
                item.Init(item.PsudeoSave(item.scriptableObject));
            }
          
            activeBeast = party[0];
            ApplyLoadedInfo();

        }
        
    }
    //Passive
    //free to swap from/to for the first time in a round
    //negative status effect(bleed heal) heals instead of damages. 

    //heal after wild encounter
    //buff with low torch light
    //heal for unspent mana
    //global discard hand
    //buffs ally damage
    //elemental damage buff for party memebr
    //weather buffs
    //rammus
    //heal for every new 
    //overworld tile you walk on
    //buff depening on what biome tile the battle takes place on
    //start combat with full block
    //repeat card cast
    //something after playing last card in hand
    //WEATHERVANE:Gains different effect depending what direction you where facing when the battle begins


    //arrow chaarcter use arrows in iventory



        //charcter gains damage mased on how much card currency they are missing
    public void Load(SaveData data)
    {
        party = new List<Beast>();
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
   
  

        for (int i = 0; i < data.party.Count; i++)  
        {
            Beast b = Instantiate(beastPrefab,transform);
            b.Init(data.party[i]);
            AddNewBeast(b);
        }
            
        //Active beast is the 0th entry of the party.
        activeBeast = party[0];
        ApplyLoadedInfo();
    }

    public void ChangePartyOrder(Beast newActiveBeast){
       
        party.RemoveAt(party.IndexOf(newActiveBeast));
        party.Insert(0,newActiveBeast);
        activeBeast = newActiveBeast;

    }
    
    void ApplyLoadedInfo()
    {
          PassiveManager.inst.OrginizePassiveActivity();
        BottomCornerBeastDisplayer.inst.ChangeActiveBeast(activeBeast);
      
        
    }

    public void AddNewBeast(Beast b)
    {
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
                { cardIDs.Add(card.Id); }
                saveData.deckIDs = cardIDs;
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
