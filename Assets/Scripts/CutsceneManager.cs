using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : Singleton<CutsceneManager>
{
    public Camera mainCam,cutSceneCam;

    public void EnterCutscene(Location location){
        // mainCam.enabled = false;
        // cutSceneCam.enabled = true;
        OverworldMovement.canMove = false;
        Interactor.inst.canInteract = false;
        DialogManager.inst.StartConversation(location.dialogToLoadIfCutscene);
    
    }

    public void LeaveCutscene(){
        // mainCam.enabled = true;
        // cutSceneCam.enabled = false;
        OverworldMovement.canMove = true;
    }
}
