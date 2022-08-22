using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInventory : Singleton<PlayerInventory>
{
    public List<BeastScriptableObject> scriptableObjects = new List<BeastScriptableObject>();
    public List<Beast> beasts = new List<Beast>();
    public List<Item> items = new List<Item>();

    public void RemoveItem(Item i)
    {
        items.Remove(i); 
    }
    

}