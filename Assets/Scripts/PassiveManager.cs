using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;


public class PassiveManager:Singleton<PassiveManager>
{
    public Dictionary<Beast,bool> dict = new Dictionary<Beast,bool>();
    public Dictionary<Beast,List<UnityAction>> actions = new Dictionary<Beast,List<UnityAction>>();
    public void AddNewBeast(Beast b)
    {
        if(!dict.ContainsKey(b))
        {dict.Add(b,false);}
        else
        {Debug.LogAssertion("Beast is already in the passive dict!");}
    }
    
    public void OrginizePassiveActivity()
    { 
        for (int i = 0; i < dict.Count; i++)
        {
            Beast b = dict.ElementAt(i).Key;
            Passive p = b.scriptableObject.beastData.passive;
        
            switch(p.operativeState)
            {
                case PassiveOperativeState.OperatesOnlyWhenActiveBeast:
                if(b == PlayerParty.inst.activeBeast)
                {
                    dict[b] = true;
                    b.scriptableObject.beastData.passive.Init(b);
                }
                else
                {
                    if(dict[b])
                    {
                        b.scriptableObject.beastData.passive.Wipe(b);
                        dict[b] = false;
                    }
                }
                break;

                case PassiveOperativeState.OperatesOnlyWhenInBackline:
                if(b != PlayerParty.inst.activeBeast)
                {
                    dict[b] = true;
                    b.scriptableObject.beastData.passive.Init(b);
                }
                else
                {
                    if(dict[b])
                    {
                        b.scriptableObject.beastData.passive.Wipe(b);
                        dict[b] = false;
                    }
                }
                break;
                
                case PassiveOperativeState.AlwaysOn:
                if(PlayerParty.inst.party.Contains(b))
                {
                    if(!dict[b])
                    {
                        dict[b] = true;
                        b.scriptableObject.beastData.passive.Init(b);
                    }
                }
                else
                {
                    b.scriptableObject.beastData.passive.Wipe(b);
                    dict.Remove(b);
                    Debug.Log(b.scriptableObject.beastData.beastName + " has been removed from the passive dictionary.");
                }
                break;
            }
        }
        
        foreach (var item in dict)
        {
            if(item.Value == false)
            {Debug.Log(item.Key.scriptableObject.beastData.beastName + "  passive is not active");}
            else
            {Debug.Log(item.Key.scriptableObject.beastData.beastName + " active");}
        }
    }

}