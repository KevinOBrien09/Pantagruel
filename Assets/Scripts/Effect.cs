using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Effect :ScriptableObject
{
    public virtual void  Use(EffectArgs args)
    {
        Debug.Log(name);
    }

   

    public virtual bool canUse(bool isPlayer){
        return true;
    }
} 

public class EffectArgs
{
    public Entity caster;
    public Entity target;
    public  bool isPlayer;
    public Card card; 
    

    public EffectArgs(Entity CASTER,Entity TARGET,bool ISPLAYER, Card CARD)
    {
        caster = CASTER;
        target =TARGET;
        isPlayer = ISPLAYER;
        card = CARD;
    
        
    }
}