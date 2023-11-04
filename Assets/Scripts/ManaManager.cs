using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ManaManager:Singleton<ManaManager>                   
{
    public ManaGem gemPrefab;
    public List<ManaGem> gems = new List<ManaGem>();
    public List<ManaGem> activeGems = new List<ManaGem>();
    public int maxMana;
    public int currentMana;
    public int maxManaBase = 8;
    public TextMeshProUGUI manaCountText;

   protected override void Awake()
    {
        base.Awake();
       LeaveBattle();
    }

    public void LeaveBattle()
    {
         maxMana = 0;
        currentMana = 0;
        //UpdateDisplay();
       
        for (int i = 0; i < gems.Count; i++)
        {gems[i].Deactivate();}
       
        activeGems.Clear();
          manaCountText.text = currentMana + "/" + maxMana;
    }

    public void IncreaseMaxMana()
    {
        if(maxManaBase != maxMana)
        {
            maxMana++;
            
            for (int i = 0; i < maxMana; i++)
            {
                if(!gems[i].active)
                {
                    gems[i].Activate();
                    activeGems.Add(gems[i]);
                }
            }
        }
        else
        {Debug.Log("Reached max base mana.");}
    }

    public void RegenMana()
    {
        currentMana = maxMana;
        UpdateDisplay();
    }

    public void Spend(int manaCost)
    {
        currentMana = currentMana- manaCost;
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        foreach (var item in activeGems)
        {item.TurnOffCircle();}

        for (int i = 0; i < currentMana; i++)
        {activeGems[i].TurnOnCircle();}

        manaCountText.text = currentMana + "/" + maxMana;
    }
}