
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/DamagePromise", fileName = "DamagePromise")]
public class DamagePromise :Promise
{
    public int howMuchDamage, howManyTurns;

    public override void Use(EffectArgs args)
    {
        base.Use(args);
  
    }

    public override void RemoveEvent(string id)
    {
        base.RemoveEvent(id);

    }

    public override void PromiseFufilled(EffectArgs OGargs, string id)
    {
        base.PromiseFufilled(OGargs, id);
        Debug.Log("Poo XD");
    }

    public override void ExecuteBadEffects(EffectArgs OGargs)
    {
        base.ExecuteBadEffects(OGargs);
        Debug.Log("BADSTUFF XC");
    }
}