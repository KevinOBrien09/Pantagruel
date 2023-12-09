using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpperLeftPanel : Singleton<UpperLeftPanel>
{
    public GameObject overworld,battle;
    public TextMeshProUGUI locName;
   
    void Start()
    {
        SwapToOverworld();
    }

    public void ChangeLocationText(MainLocation mainLocation){
        locName.text = mainLocation.locName;
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
