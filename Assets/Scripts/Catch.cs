using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Catch", fileName = "Catch")]
public class Catch :Effect
{
    public int ballPower;
   

    public override void Use(Entity caster,Entity target,bool isPlayer, List<Entity> casterTeam = null ,List<Entity> targetTeam = null)
    {
        BattleTicker.inst.Type("The beast....");

        CardManager.inst.DeactivateHand();
   
        if(CatchManager.IsCaptureSuccessful(ballPower,(Beast)target))
        {
            PlayerParty.inst.AddNewBeast((Beast)target);
            PassiveManager.inst.OrginizePassiveActivity();
            Beast b = RivalBeastManager.inst.activeBeast;
            RivalBeastManager.inst.RemoveActiveBeast();
            if(BattleManager.inst.partyHasValidMember(RivalBeastManager.inst.currentParty))
            {
                Debug.Log("Force Swap");
            }
            else{

                CardManager.inst.StartCoroutine(q());
                IEnumerator q(){
                    yield return new WaitForSeconds(1f);
                    b.animatedInstance.Dissolve();
                    BattleTicker.inst.Type("is captured!");
                    
                    BattleManager.inst.Win();
                   
                }
               
            }
        }
        else
        {
           
            CardManager.inst.StartCoroutine(q());
            IEnumerator q(){
                yield return new WaitForSeconds(.5f);
                BattleTicker.inst.Type("escaped...");
                yield return new WaitForSeconds(1f);
                CardManager.inst.ActivateHand();
            }
        }

       
    }

}