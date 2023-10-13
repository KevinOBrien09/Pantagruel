using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RivalBeastManager:Singleton<RivalBeastManager>                   
{
    public HealthBar healthBar;
    public Beast activeBeast;
    public Beast beastPrefab;
    public List<Beast> currentParty = new List<Beast>();

    public void TrainerInit(){

    }

    public void CreateEnemyParty(List<BeastScriptableObject> beasts)
    {
        foreach (var item in beasts)
        {
            Beast b = Instantiate(beastPrefab,transform);
            b.Init(b.PsudeoSave(item));
            currentParty.Add(b);
        }
        EnemyAI.inst.CreateDecks(currentParty);
    }

    public void SwapActiveBeast(Beast newActiveBeast)
    {
        activeBeast = newActiveBeast;
        EnemyAI.inst.SwapActiveBeast(newActiveBeast);
        CreateActiveBeastGraphic(newActiveBeast);
        healthBar.beast = activeBeast;
        activeBeast.currentHealthBar = healthBar;
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
        BattleGraphicManager.inst.Wipe();
        activeBeast = null;
        currentParty.Clear();
        healthBar.beast = null;
    }

   
}