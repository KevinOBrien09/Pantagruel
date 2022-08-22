using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ItemCaster : Singleton<ItemCaster>
{
    public void Use(Item i)
    {
        switch (i)
        {
            case ItemHeal h:
            StartCoroutine(Heal(i));
            break;
            case CaptureBeastItem captureBeastItem:
            Debug.Log("Capture");
            break;
            case Item it:
            Debug.Log("Base");
            break;
            
          
        }

    }


    IEnumerator Heal(Item i)
    {
        ItemHeal ih = (ItemHeal)i;
        UIManager.inst.Normal();
        BattleManager.inst.skillHandler.DeActivateSkillButtons();
        yield return new WaitForSeconds(.2f);
        MainTextTicker.inst.Type(i.data.itemName+".");
        yield return new WaitForSeconds(.2f);
        BattleManager.inst.activePlayerBeast.OnHeal(ih.healAmount);
        yield return new WaitForSeconds(.2f);
        BattleManager.inst.StartCoroutine(BattleManager.inst.EndOfTurnDelay(1));
    }


}