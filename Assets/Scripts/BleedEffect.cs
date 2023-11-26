using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/Bleed", fileName = "Bleed")]
public class BleedEffect : StatusEffectEffect
{
    public override void Trigger(Beast infected,bool isPlayer){
       infected.Poison(isPlayer);
    }
}