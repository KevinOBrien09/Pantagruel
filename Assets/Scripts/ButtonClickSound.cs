using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class ButtonClickSound : MonoBehaviour
{
    public void Click(){
        AudioManager.inst.GetSoundEffect().Play(SystemSFX.inst.clickButton);
    }
}