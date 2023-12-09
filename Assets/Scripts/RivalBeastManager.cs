using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RivalBeastManager:Singleton<RivalBeastManager>                   
{
    public EnemyBeastHealthbar healthBar;
    public Beast activeBeast;
    public Beast beastPrefab;
    public List<Beast> currentParty = new List<Beast>();

    

    public void CreateEnemyParty(List<BeastScriptableObject> beasts)
    {
        foreach (var item in beasts)
        {
            Beast b = Instantiate(beastPrefab,transform);
            EXP e = new EXP(); 
            e.PsudeoLevel((int)Random.Range(LocationManager.inst.currentSubLocation.levelRange.x,LocationManager.inst.currentSubLocation.levelRange.y),b);
            b.Init(b.PsudeoSave(item));
            b.ownership = EntityOwnership.ENEMY;
            b.FirstHealthInit(e);
            currentParty.Add(b);
        }
        EnemyAI.inst.CreateDecks(currentParty);
    }


    public void SwapActiveBeast(Beast newActiveBeast)
    {
        activeBeast = newActiveBeast;
        EnemyAI.inst.SwapActiveBeast(newActiveBeast);
        CreateActiveBeastGraphic(newActiveBeast);
        healthBar.entity = activeBeast;
        BattleField.inst.enemyBeast.InitBeast(newActiveBeast);

        activeBeast.currentHealthBars.Add(healthBar);
        healthBar.onInit.Invoke();
      
    }

    public void RemoveActiveBeast(){
        currentParty.Remove(activeBeast);
        activeBeast = null;
    }

    public void CreateActiveBeastGraphic(Beast b)
    {
        BattleGraphicManager.inst.Init(b.scriptableObject);
    }

    public void Wipe()
    {
        foreach (var item in currentParty)
        {
            Destroy(item.gameObject);
        }
        BattleGraphicManager.inst.Wipe();
        activeBeast = null;
        currentParty.Clear();
        healthBar.entity = null;
    }

   
}