using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperLeftPanel : Singleton<UpperLeftPanel>
{
    public GameObject overworld,battle;
   
    void Start()
    {
        SwapToOverworld();
    }

    public void SwapToBattle()
    {
       overworld.gameObject.SetActive(false);
       battle.gameObject.SetActive(true); 
    }

    public void SwapToOverworld()
    {
        overworld.gameObject.SetActive(true);
       battle.gameObject.SetActive(false); 
    }

   
}
