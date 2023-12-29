using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    public Dialog dialog;
    public List<Item> items =new List<Item>();
    public override void Go()
    {
        Item item = items[Random.Range(0,items.Count)];
        Inventory.inst.itemPickedUp = item;
        // if(TutorialManager.inst.ExecuteTutorial(TutorialEnum.CARDS)){
        //     DialogManager.inst.StartConversation(TutorialManager.inst.GetTutorial(TutorialEnum.CARDS));
        // }
        // else{
             DialogManager.inst.StartConversation(dialog);
        // }

        Interactor.inst.interactText.SetActive(true);
        Inventory.inst.AddItem(item);
        Interactor.inst.RenableInteraction();
        Destroy(transform.parent.gameObject);
    }

}