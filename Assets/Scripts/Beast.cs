 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beast : MonoBehaviour
{
    public BeastScriptableObject scriptableObject;
    public float currentHealth;
    public Deck deck;
    public int shields;
    public HealthBar currentHealthBar;
    public BeastAnimatedInstance animatedInstance;
    public bool KO;


    public void Init(BeastSaveData beastSaveData)
    {
        scriptableObject = BeastBank.inst.beastDict[beastSaveData.beastiaryID];
        gameObject.name = scriptableObject.beastData.beastName;
        currentHealth = beastSaveData.currentHealth;
        foreach (var id in beastSaveData.deckIDs)
        {
            if(BeastBank.inst.cardDict.ContainsKey(id))
            {deck.cards.Add(BeastBank.inst.cardDict[id]);}
            else
            {Debug.LogError("Database does not contain card id : " + id);}
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth = currentHealth-amount;
        if(currentHealth <0)
        {
            currentHealth = 0;
        }
        
        if(animatedInstance != null){
            animatedInstance.TakeDamage();
        }
        
        if(currentHealth ==0){
            KO = true;
            animatedInstance.Dissolve();
        }
        currentHealthBar.onHit.Invoke();
        BattleManager.inst.CheckIfGameContinues();
       
    }

    public BeastSaveData PsudeoSave(BeastScriptableObject bso)
    {
        BeastSaveData bsd = new BeastSaveData();
        bsd.beastiaryID = bso.beastData.bestiaryID;
        bsd.currentHealth = bso.beastData.stats.maxHealth;
        List<string> cardIDs = new List<string>();
        foreach (var card in bso.beastData.wildDeck.deck.cards)  //ONLY DOES WILD DECK ATM
        { cardIDs.Add(card.Id); }
        bsd.deckIDs = cardIDs;
        return bsd;
    }
    
}
