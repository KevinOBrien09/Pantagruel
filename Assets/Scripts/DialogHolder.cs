using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHolder: Interactable
{
    public Dialog dialog;

    public override void Go(){
         DialogManager.inst.StartConversation(dialog);
    }


}