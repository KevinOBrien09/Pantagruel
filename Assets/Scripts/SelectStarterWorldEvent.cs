using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectStarterWorldEvent : WorldEvent
{
    public override void Go(){
        StarterSelector.inst.Open();
    }
}