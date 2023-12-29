using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPickUp : Interactable
{
    public Dialog dialog;
    public List<Card> cards =new List<Card>();
    public override void Go()
    {
         Card card = cards[Random.Range(0,cards.Count)];
        CardCollection.inst.cardPickUp = card;
        if(TutorialManager.inst.ExecuteTutorial(TutorialEnum.CARDS)){
            DialogManager.inst.StartConversation(TutorialManager.inst.GetTutorial(TutorialEnum.CARDS));
        }
        else{
             DialogManager.inst.StartConversation(dialog);
        }

       
        CardCollection.inst.AddCard(card);
        Destroy(transform.parent.gameObject);
    }

}