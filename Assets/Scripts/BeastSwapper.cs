using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BeastSwapper : Singleton<BeastSwapper>
{
    public bool Swap(Beast b)
    {
        if(PlayerParty.inst.activeBeast == b){ 
            Debug.Log("Beast is the same fuck off");
            return false;
        }
      
        Beast newActiveBeast = b;
       
        BottomCornerBeastDisplayer.inst.ChangeActiveBeast(newActiveBeast,true);
        PlayerParty.inst.ChangePartyOrder(newActiveBeast);
        PassiveManager.inst.OrginizePassiveActivity();
        return true;
        
    }
    
   

}
