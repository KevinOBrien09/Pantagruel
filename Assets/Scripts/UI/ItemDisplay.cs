using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ItemDisplay : Singleton<ItemDisplay>
{
    [SerializeField] Transform slotHolder;
    [SerializeField] GameObject itemSlotPrefab;
    [SerializeField] TextMeshProUGUI nameText,descText;
    [SerializeField] Image picture;
    [SerializeField] Button useButton;
    public List<ItemSlot> itemSlots = new List<ItemSlot>();
    public ItemSlot currentlySelectedSlot;

    void Start(){
        Init(PlayerInventory.inst.items);
    }
    public void Init (List<Item> items)
    {
        foreach (var item in itemSlots)
        {
            Destroy(item.gameObject);
        }
        itemSlots.Clear();

        for (int i = 0; i < items.Count; i++)
        {
            GameObject g = Instantiate(itemSlotPrefab,slotHolder);
            ItemSlot s = g.GetComponent<ItemSlot>();
            itemSlots.Add(s);
            s.Apply(items[i]);
            if(i==0){
                UpdateMainDisplay(s);
            }
        }
    }

    public void UpdateMainDisplay(ItemSlot i)
    {
        ItemData d = i.item.data;
        picture.sprite = d.picture;
        nameText.text = d.itemName;
        descText.text = d.desc;
        if(d.isConsumable)
        {
            useButton.gameObject.SetActive(true);
            useButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(()=> {i.item.Use(); Reset(i); });
        }
        else
        {
            useButton.gameObject.SetActive(false);
            useButton.onClick.RemoveAllListeners();
        }
    }

    void Reset(ItemSlot i)
    {
        PlayerInventory.inst.RemoveItem(i.item);
        Init(PlayerInventory.inst.items);
    }
    

}