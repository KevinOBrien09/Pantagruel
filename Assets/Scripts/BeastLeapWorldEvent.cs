using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeastLeapWorldEvent : WorldEvent
{
    public GameObject objectToMove;
    public Vector3 preparePos;
    public Vector3 endPos;
    public Beast encounteredBeast;
    public SoundData sfx;
    public float back,forward;
    public override void Go(){
        if(!objectToMove.activeSelf){
            objectToMove.SetActive(true);
        }
        
        objectToMove.transform.DOMove(preparePos,back).OnComplete(()=>{
            AudioManager.inst.GetSoundEffect().Play(sfx);
            BattleManager.inst.StartBattle(BattleType.Wild);
        objectToMove.transform.DOMove(endPos,forward);
   });
       
     
    }

    [ContextMenu("Get Pos")]
    void DoSomething()
    {
        if(objectToMove != null){
            endPos = objectToMove.transform.position;
        }
    }

}