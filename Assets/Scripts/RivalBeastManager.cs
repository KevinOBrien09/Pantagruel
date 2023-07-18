using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RivalBeastManager:Singleton<RivalBeastManager>                   
{
    public HealthBar healthBar;
    public Beast currentBeast;
    public Beast beastPrefab;

    public void EnterBattle()
    {
        BeastScriptableObject encounteredBeast = LocationManager.inst.GetEncounter(PlayerManager.inst.movement.GetBiome()) ;
        BattleGraphicManager.inst.Init(encounteredBeast);   
        currentBeast = Instantiate(beastPrefab,transform);
        currentBeast.Init(currentBeast.PsudeoSave(encounteredBeast));
        healthBar.beast = currentBeast;
        currentBeast.currentHealthBar = healthBar;
        healthBar.onInit.Invoke();
    }

   
}