using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using TMPro;

public class BeastTargeterButton :MonoBehaviour                
{
    public enum TargetJob{SWAP,ATTACK}
    public TextMeshProUGUI beastName;
    public TargetJob targetJob;
    public Image beastPicture;
    Beast beast;
    public void Init(Beast b,TargetJob job)
    {
        beastPicture.sprite = b.scriptableObject.beastData.uiPicture;
        beastName.text = b.scriptableObject.beastData.beastName;
        targetJob = job;
        beast = b;
    }

    public void Click()
    {
        switch(targetJob)
        {
            case TargetJob.SWAP:
            BeastTargeter.inst.CommenceSwap(beast);
            break;
            case TargetJob.ATTACK:
            Debug.Log("Attack!");
            break;
            
        }
    }
}