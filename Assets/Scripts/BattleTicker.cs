using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using KoganeUnityLib;

public class BattleTicker : Singleton<BattleTicker>
{
    public TMP_Typewriter txt;

    public void Type(string message)
    {
        txt.Play(message,60,(()=>{}));
    }

}