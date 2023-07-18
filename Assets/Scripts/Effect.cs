using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Effect :ScriptableObject
{
    public virtual void  Use(Beast caster,Beast target, List<Beast> casterTeam = null ,List<Beast> targetTeam = null)
    {
        Debug.Log(name);
    }

}