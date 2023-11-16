 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beast : Entity
{
    public BeastScriptableObject scriptableObject;
    public Deck deck;
    public int shields;
    public EXP exp = new EXP();
    public Stats statMods = new Stats();
    
    
    
    public void Init(BeastSaveData beastSaveData)
    {
        scriptableObject = BeastBank.inst.beastDict[beastSaveData.beastiaryID];
        gameObject.name = scriptableObject.beastData.beastName;
        currentHealth = beastSaveData.currentHealth;
        exp = new EXP();
        exp.Load( beastSaveData.exp);
       
         
        foreach (var id in beastSaveData.deckIDs)
        {
            if(BeastBank.inst.cardDict.ContainsKey(id))
            {deck.cards.Add(BeastBank.inst.cardDict[id]);}
            else
            {Debug.LogError("Database does not contain card id : " + id);}
        }
    }

    public void FirstHealthInit(EXP e){
        exp = e;
        currentHealth = stats().maxHealth;

    }
    public void LevelUp()
    {
        if(scriptableObject!=null){
        currentHealth+= scriptableObject.beastData.statGrowthPerLevel.maxHealth;
        foreach (var item in currentHealthBars)
        {item.onHit.Invoke(); }
        }

        if(PlayerParty.inst.activeBeast == this){ 
            
            BottomCornerBeastDisplayer.inst.RefreshLevel();
        }
       
    }




    public BeastSaveData PsudeoSave(BeastScriptableObject bso)
    {
        BeastSaveData bsd = new BeastSaveData();
        bsd.beastiaryID = bso.beastData.bestiaryID;
        scriptableObject = bso;//???
         
        List<string> cardIDs = new List<string>();
        foreach (var card in bso.beastData.wildDeck.deck.cards)  //ONLY DOES WILD DECK ATM
        { cardIDs.Add(card.Id); }
        bsd.deckIDs = cardIDs;
        return bsd;
    }
    
    public override Stats stats()
    {
        Stats s = new Stats(scriptableObject.beastData.baseStats);
        for (int i = 0; i < exp.level; i++)
        {s.StackStats(scriptableObject.beastData.statGrowthPerLevel);}
        s.StackStats(statMods);
        return s;
    }
}
