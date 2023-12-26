using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ManaHandler:MonoBehaviour             
{
    public int currentMana,maxMana;
    public int currentRegen;
    public Image manaBarFill;
    public TextMeshProUGUI manaText;
    Beast beast;

    public void SwapBeast(Beast b){
        maxMana = (int) b.stats().maxMana;
        currentMana = maxMana;
        ////2;
        beast = b;
        Refresh();
   
    }

    public void Wipe(){

    }

    public void Gain(float gain){
        float gainedMana = Mathf.Min(beast. stats().maxMana -  currentMana, gain);
        currentMana += (int) gainedMana;
        Refresh();
    }

    public void Spend(int cost){
        currentMana -= cost;

        Refresh();
    }

    public void Refresh()
    {
        manaBarFill.DOFillAmount((float) currentMana/(float) maxMana,.25f);
        manaText.text = currentMana.ToString() + "/" + maxMana.ToString();
    }
}