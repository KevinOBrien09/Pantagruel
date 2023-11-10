using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Beast Passive/115")]
public class PassiveOneHundredAndFifteen : Passive
{
    public Card card;
    
    public override void Init(Beast b)
    {
        UnityAction u = () =>CardManager.inst.CreateVapourCard(card);
        PassiveManager.inst.actions.Add(b,new List<UnityAction>());
        PassiveManager.inst.actions[b].Add(u);
        EventManager.inst.AssignEvent(EventEnum.onPlayerDrawingCardFirstTimeInTurn,u);
        //onPlayerDrawingCardFirstTimeInTurn.AddListener(u);

    }

    public override void Wipe(Beast b)
    {
        foreach (var item in PassiveManager.inst.actions[b])
        {
            EventManager.inst.RemoveEvent(EventEnum.onPlayerDrawingCardFirstTimeInTurn,item);
            //onPlayerDrawingCardFirstTimeInTurn.RemoveListener(item);
        }
        PassiveManager.inst.actions.Remove(b);
           
    }
    
    
}
