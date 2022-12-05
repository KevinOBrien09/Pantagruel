using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class BeastCaptureManager : Singleton<BeastCaptureManager>
{
    public void StartCapture(int catchPercent)
    {
        Debug.Log("trying to catch" + BattleManager.inst.activeEnemyBeast);
    }



}