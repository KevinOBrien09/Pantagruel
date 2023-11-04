 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LeftPanel: Singleton<LeftPanel>                  
{
    public GameObject battle;
    public GameObject overworld;

    void Start(){
        SwapToOverworld();
    }


    public void SwapToOverworld()
    {
        battle.SetActive(false);
        overworld.SetActive(true);
    }

    public void SwapToBattle()
    {
        battle.SetActive(true);
        overworld.SetActive(false);
    }

}