using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndTurnButton : Singleton<EndTurnButton>
{
    public Button button;
    public SoundData click;
    public void Deselect(){
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void PlaySFX(){
        AudioManager.inst.GetSoundEffect().Play(click);
    }

   

    public void Deactivate()
    {
        button.interactable = false;
    }

    public void Reactivate()
    {
        if(TutorialManager.inst.isInBasics3()){
            if(CardManager.inst.hand.Count == 0){
                TutorialManager.inst.ProcessEvent("TURNBUTTON");
               button.interactable = true;  
            }
            return;
        }
        if(BattleManager.inst.inTutorial)
        {
            return;
        }
     
         button.interactable = true;
    }

}