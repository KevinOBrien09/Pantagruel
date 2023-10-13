using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastBank : Singleton<BeastBank>
{
    public List<BeastScriptableObject> bank = new List<BeastScriptableObject>();
    public List<Card> cardBank = new List<Card>();
    public Dictionary<int,BeastScriptableObject> beastDict = new Dictionary<int, BeastScriptableObject>();
    public Dictionary<string,Card> cardDict = new Dictionary<string,Card>();
    
    protected override void Awake()
    {
        base.Awake();
        foreach (var item in bank)
        {beastDict.Add(item.beastData.bestiaryID,item);}

        foreach (var item in cardBank)
        {cardDict.Add(item.Id,item);}
    }
}
