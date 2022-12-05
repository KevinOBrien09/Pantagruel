using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PlayerInventory : Singleton<PlayerInventory>
{
    public List<BeastScriptableObject> scriptableObjects = new List<BeastScriptableObject>();
    public List<Beast> beasts = new List<Beast>();
    public List<Item> items = new List<Item>();
    public Beast activeBeast;
    [SerializeField] GameObject beastPrefab;
    public List<BeastInventorySlot> beastInventorySlots = new List<BeastInventorySlot>();

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        foreach (var item in scriptableObjects)
        {
           GameObject g = Instantiate(beastPrefab,transform);
           Beast b = g.GetComponent<Beast>();
           b.Init(item,Alliance.Player);
           beasts.Add(b);
           
            g.SetActive(false);
            foreach (var slot in beastInventorySlots)
           {
                if(slot.beast == null){
                    slot.ApplyBeast(b);
                    break;
                }
           }

        }
        UpdateBeastStatus();
        ChangeActiveBeast(beasts[0]);
    }

    public void ChangeActiveBeast(Beast b)
    {
        int i = beasts.IndexOf(b);
        activeBeast?.gameObject.SetActive(false);
        activeBeast = beasts[i];
        b.gameObject.SetActive(true);
        BattleManager.inst.activePlayerBeast = activeBeast;
        BattleManager.inst.ApplyPlayerBeastInfo(activeBeast);
    }

    public void UpdateBeastStatus(){
        foreach (var item in beastInventorySlots)
        {
            item.ApplyBeast(item.beast);
        }
    }

    public void RemoveItem(Item i)
    {
        items.Remove(i); 
    }
    

}