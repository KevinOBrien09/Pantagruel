using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndTurnButton : Singleton<EndTurnButton>
{
    public Button button;
    public void Deselect(){
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Deactivate()
    {
        button.interactable = false;
    }

    public void Reactivate()
    {
         button.interactable = true;
    }

}