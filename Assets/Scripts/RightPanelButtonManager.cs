using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPanelButtonManager : Singleton<RightPanelButtonManager>
{
   public GameObject overworldButtons,battleButtons;

   void Start(){
      SwapToOverworld();
   }


   public void SwapToBattle(){
    overworldButtons.SetActive(false);
    battleButtons.SetActive(true);
   }

    public void SwapToOverworld(){
    overworldButtons.SetActive(true);
    battleButtons.SetActive(false);
   }
}
