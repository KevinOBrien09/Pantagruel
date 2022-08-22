using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerDownHandler
{
    public Item item;
    public Image image;
    public Image border;
    public void Apply(Item data)
    {
        item = data;
        image.sprite = item.data.picture;
        // nameText.text = data.itemName;
        // descText.text = data.desc;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left | eventData.button == PointerEventData.InputButton.Middle)
        {
            Activate();
        }
    }

    public void Activate()
    {
        if(ItemDisplay.inst.currentlySelectedSlot != null){
        ItemDisplay.inst.currentlySelectedSlot.border.color = Color.white;
        }
        
        border.color = Color.red;
        ItemDisplay.inst.currentlySelectedSlot = this;
        ItemDisplay.inst.UpdateMainDisplay(this);
    }
}
