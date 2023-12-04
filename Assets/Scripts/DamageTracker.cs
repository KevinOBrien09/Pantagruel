using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamagePromiseRecord{
   public DamagePromise promise;
   public int howMuchDamage;

}


public class DamageTracker:Singleton<DamageTracker>                   
{
    public GenericDictionary<string,DamagePromiseRecord> playerDict = new GenericDictionary<string,DamagePromiseRecord>();
    
    public GenericDictionary<string,DamagePromiseRecord> enemyDict = new GenericDictionary<string,DamagePromiseRecord>();

    public void AddEffect(DamagePromise e,string id,bool isPlayer)
    {
        if(isPlayer)
        {
            if(!playerDict.ContainsKey(id))
            {
                DamagePromiseRecord dpr = new DamagePromiseRecord();
                dpr.promise = e;
                playerDict.Add(id,dpr);
            }
        }
        else
        {
            if(!enemyDict.ContainsKey(id))
            {
                DamagePromiseRecord dpr = new DamagePromiseRecord();
                dpr.promise = e;
                enemyDict.Add(id,dpr);
            }
        }
    }

    public void RecordPlayerDamage(int dmg)
    {
        foreach (var item in playerDict)
        {
            item.Value.howMuchDamage += dmg;
        }
    }

    public void RecordEnemyDamage(int dmg)
    {
        foreach (var item in enemyDict)
        {
            item.Value.howMuchDamage += dmg;
        }
    }

    public void Wipe(){
        playerDict.Clear();
        enemyDict.Clear();
    }

    public void RemoveEffect(string id)
    {
        if(playerDict.ContainsKey(id))
        {playerDict.Remove(id);}

        if(enemyDict.ContainsKey(id))
        {enemyDict.Remove(id);}
    }

    public int GetDamage(string id)
    {
        if(playerDict.ContainsKey(id))
        {return playerDict[id].howMuchDamage;}

        if(enemyDict.ContainsKey(id))
        { return enemyDict[id].howMuchDamage; }
        Debug.LogAssertion("ID NOT FOUND!!!");
        return 0;
    }   

  

}