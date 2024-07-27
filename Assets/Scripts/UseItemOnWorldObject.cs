using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
public class UseItemOnWorldObject: Interactable
{
    public WorldEvent worldEvent;
    public Dialog solved,unsolved;
    public Item requiredItem;

    public override void Go()
    {  
        if(!Inventory.inst.dict.ContainsKey(requiredItem))
        {
            DialogManager.inst.StartConversation(unsolved);
        }
        else
        {
            worldEvent?.Go();
            DialogManager.inst.StartConversation(solved);
           Destroy(gameObject);
        }
     

    } 
}