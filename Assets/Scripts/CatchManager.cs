using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class CatchManager 
{

    public static bool IsCaptureSuccessful(int ballPower, Beast target)
    {
        BeastData d = target.scriptableObject.beastData;
        if(target.currentHealth == target.stats().maxHealth)
        {
            Debug.Log("Target is full health, catch not possible.");
            return false;
        }
        //status effects
        int catchChance = 0;
        int h = 0;
        float p = target.currentHealth /(float)target.stats().maxHealth;
        p = p*100;
        p = Mathf.Floor(p);
        h = 100 - (int)p;


        catchChance += h;
        
        int catchModifer = d.catchMod - ballPower;
        int f = Random.Range(0,105+catchModifer);

        if(catchChance >= f)
        {
            Debug.Log("Catch");
             return true;
        }
        else{
            Debug.Log("Escape");
             return false;
        }
    }

  

}