using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Effect :ScriptableObject
{
    public virtual void  Use(Entity caster,Entity target,bool isPlayer, List<Entity> casterTeam = null ,List<Entity> targetTeam = null)
    {
        Debug.Log(name);
    }

    public virtual bool canUse(bool isPlayer){
        return true;
    }

}