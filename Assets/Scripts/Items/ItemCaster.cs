using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ItemCaster : Singleton<ItemCaster>
{
    public void Use(Item i)
    {
        UIManager.inst.Normal();
        BattleManager.inst.skillHandler.DeActivateSkillButtons();
        switch (i)
        {
            case ItemHeal h:
            StartCoroutine(Heal(i));
            break;
            case CaptureBeastItem captureBeastItem:
            MainTextTicker.inst.Type(i.data.itemName+".");
            CaptureBeastItem cbi = (CaptureBeastItem)i;
            BeastCaptureManager.inst.StartCapture(cbi.catchPercent);
            break;
            case Item it:
            Debug.Log("Base");
            break;
            
          
        }

    }
    
    IEnumerator Heal(Item i)
    {
        ItemHeal ih = (ItemHeal)i;
     
     
        yield return new WaitForSeconds(.2f);
        MainTextTicker.inst.Type(i.data.itemName+".");
        yield return new WaitForSeconds(.2f);
        BattleManager.inst.activePlayerBeast.OnHeal(ih.healAmount);
        yield return new WaitForSeconds(.2f);
        BattleManager.inst.StartCoroutine(BattleManager.inst.EndOfTurnDelay(1));
    }


}