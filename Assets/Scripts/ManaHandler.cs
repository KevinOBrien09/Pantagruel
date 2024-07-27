using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ManaHandler:MonoBehaviour             
{
    public int currentMana;
    public int currentRegen;
    public Image manaBarFill;
    public TextMeshProUGUI manaText,regenText;
    Beast beast;

    public void SwapBeast(Beast b)
    {
        beast = b;
        currentMana = (int) beast.stats().maxMana;
        Refresh();
    }

    public void Wipe(){

    }

    public void Regen(float gain){
        if(currentMana > beast. stats().maxMana ){
            currentMana = (int) beast. stats().maxMana;
            Refresh();
            return;
        }
        float gainedMana = Mathf.Min(beast. stats().maxMana -  currentMana, gain);
        currentMana += (int) gainedMana;
        Refresh();
    }
    
    public void Spend(int cost){
        currentMana -= cost;

        Refresh();
    }

    public void Refresh()
    {  int x = (int) beast.stats().maxMana;
        manaBarFill.DOFillAmount((float) currentMana/(float)x,.25f);
        if(regenText != null){
   regenText.text = "INT: " + (int) beast.stats().manaRegen;
        }
   
        manaText.text = currentMana.ToString() + "/" + x .ToString();
    }
}