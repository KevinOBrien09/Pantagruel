using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName ="Items/Heal")]
public class ItemHeal : Item
{
    public int healAmount;

    public override void Use()
    {
        ItemCaster.inst.Use(this);
    }
}