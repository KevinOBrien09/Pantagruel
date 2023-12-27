using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlayerRunWorldEvent : WorldEvent
{
    public Transform goal;
    public override void Go()
    {
        Transform player = PlayerManager.inst.movement.transform;
        PlayerManager.inst.movement.moveSpeed = .1f;
        Forwards();

        void Forwards()
        {
            
            PlayerManager.inst.movement.StartMove(Dir.Forwards);
            StartCoroutine (q());
           

            IEnumerator q(){
                yield return new WaitForSeconds(.125f);
                if(player.transform.position != goal.position)
                {
                    Forwards();
                } 
                else{
                    Debug.Log("reached goal");
                    PlayerManager.inst.movement.moveSpeed =   PlayerManager.inst.movement.ogMoveSpeed;
                }

          
                
            } 
        }
    }
}