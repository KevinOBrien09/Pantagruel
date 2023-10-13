using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class Inventory:Singleton<Inventory>                   
{
    public Dictionary<Item,ItemStack> dict = new Dictionary<Item, ItemStack>();
    public List<Item> startingItems = new List<Item>();
    public ItemStack prefab;
    public RectTransform holder;

    public List<ItemStack> itemsUsedThisTurn = new List<ItemStack>();

    void Start()
    {
        foreach(var item in startingItems)
        {
            AddItem(item);
        }

        DisableItemDragOnAll();
        
    }

    public void DisableItemDragOnAll()
    {
        foreach (var item in dict)
        {
            if(item.Value.item.battleCastableOnly){
 item.Value.ToggleInteractable(false);
            }
           
            
        }
    }

    public void EnableItemDragOnAll()
    {
        foreach (var item in dict)
        {
            if(item.Value.item.battleCastableOnly){
 item.Value.ToggleInteractable(true);
            }
           
            
        }
    }

    public void AddItem(Item item)
    {
        if(!dict.ContainsKey(item))
        {
            ItemStack stack = Instantiate(prefab,holder);
            stack.Init(item);
            dict.Add(item,stack);
        }
        dict[item].Add();
    }

    public void DeactivateDrag()
    {
        foreach (var item in dict)
        {
          item.Value.ToggleInteractable(false);
        }
    }

    public void ActivateDrag()
    {
        foreach (var item in dict)
        {
          item.Value.ToggleInteractable(true);
        }
    }

    public void RemoveItem(Item item)
    {
        if(dict.ContainsKey(item))
        {
            dict[item].Remove();
            if(dict[item].amount <= 0)
            {
                Destroy(dict[item].gameObject);
                dict.Remove(item);
            }
        }
        else
        {Debug.LogWarning(item.itemName + ": not found in Dictionary");}
    }
}

