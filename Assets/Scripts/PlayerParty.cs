using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerParty : MonoBehaviour
{
    public List<Beast> party = new List<Beast>(); //does not contain activeBeast
    
    public Beast activeBeast;
    public Beast beastPrefab;
    
    void Start()
    {
        if(!PlayerManager.inst.load)
        { 
            
            activeBeast.Init(activeBeast.PsudeoSave(activeBeast.scriptableObject));

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

    public void Load(SaveData data)
    {
        party = new List<Beast>();
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
        if(data.activeBeast !=null)
        {
            activeBeast = Instantiate(beastPrefab,transform);
            activeBeast.Init(data.activeBeast);
        }
        else   
        {Debug.LogWarning("Active Beast is null.");}
      
      
        foreach (var item in data.party)
        {
            Beast b = Instantiate(beastPrefab,transform);
            b.Init(item);
            party.Add(b);

        }
        ApplyLoadedInfo();
    }
    
    void ApplyLoadedInfo()
    {
        BottomCornerBeastDisplayer.inst.ChangeActiveBeast(activeBeast);
        
       
       
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
                List<int> cardIDs = new List<int>();
                foreach (var card in b.deck.cards)
                { cardIDs.Add(card.cardID); }
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
