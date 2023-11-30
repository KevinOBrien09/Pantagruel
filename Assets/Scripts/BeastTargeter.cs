using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class BeastTargeter:Singleton<BeastTargeter>                   
{
    public BeastTargeterButton buttonPrefab;
    public Transform buttonHolder;
    public GameObject cards;
    public List<BeastTargeterButton> buttons = new List<BeastTargeterButton>();
    public void Open(List<Beast> B)
    {BattleManager.inst.FUCKOFF = true;
 cards.SetActive(false);
       EndTurnButton.inst.Deactivate();
        foreach (var item in B)
        {
            BeastTargeterButton btb =  Instantiate(buttonPrefab,buttonHolder);
            btb.Init(item,BeastTargeterButton.TargetJob.SWAP);
            buttons.Add(btb);
        }
    }

    public void CommenceSwap(Beast b)
    {
        BeastSwapper.inst.Swap(b);
        cards.SetActive(true);
        foreach (var item in buttons)
        {Destroy(item.gameObject);}
        buttons.Clear();
        BattleManager.inst.FUCKOFF = false;
        BattleField.inst.SetPlayerBeastIcon(b);
        BattleManager.inst.EndTurn();
        CardManager.inst.SwitchBeast(b);
    }

}