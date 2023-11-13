using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[System.Serializable]
public class ItemStack:MonoBehaviour
{
    public int amount;
    public Item item;
    public Image[] pictures;
    public TextMeshProUGUI value;
    public  ItemDragBehaviour dragBehaviour;
    public void Init(Item newItem)
    {
        pictures[0].sprite = newItem.picture;
        pictures[1].sprite = newItem.picture;
        item = newItem;
    }

    public void Add()
    {
        amount++;
        value.text = amount.ToString();
    }

    
    public void Remove()
    {
        amount--;
        value.text = amount.ToString();
    }

    public bool canBeUsed()
    {
        if(Inventory.inst.itemsUsedThisTurn.Contains(this) && item.only1PerTurn){
            return false;
        }
        return item.castable;
    }

    public void CheckIfZero()
    {
        if(amount == 0)
        {
            pictures[1].enabled = false;
        }
    }

    public void ToggleInteractable(bool b)
    {
        float fade = .3f;
        if(b == true){
            fade = 1;
        }
        pictures[1].enabled = b;
        pictures[0].DOFade(fade,.2f);
        dragBehaviour.interactable = b;
    }


    public void CheckIfOtherPictureWasDisabled()
    {
        if(!pictures[1].enabled)
        {
            pictures[1].enabled = true;
        } 
    }

    public void Use()
    {
        dragBehaviour.Reset();
      
        if(item.only1PerTurn){
        ToggleInteractable(false);
 
        }
        Inventory.inst.itemsUsedThisTurn.Add(this);
        Inventory.inst.RemoveItem(item);

        foreach (var item in item.effects)
        {
            EffectArgs args = new EffectArgs(PlayerParty.inst.activeBeast,RivalBeastManager.inst.activeBeast,true,null,null,-1);
            item.Use(args);
        }
        
        Debug.Log(item.itemName);
    }

}