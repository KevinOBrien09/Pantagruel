using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName ="Items/Basic")]
public class Item : ScriptableObject
{
   public ItemData data;

   public virtual void Use()
   {
        ItemCaster.inst.Use(this);
   }
}
[System.Serializable]
public class ItemData
{
    public string itemName;
    public Sprite picture;
    [TextArea] public string desc;
    public bool isConsumable;

    public ItemData(ItemData d)
    {
        itemName = d.itemName;
        picture = d.picture;
        desc = d.desc;
        isConsumable = d.isConsumable;
    }

}
