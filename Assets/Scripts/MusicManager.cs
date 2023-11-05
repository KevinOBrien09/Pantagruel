using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] AudioSource dungeon;
    [SerializeField]  AudioSource battle;
    [SerializeField]  AudioSource reward;
    float battlevol,dungvol,rewardVol;
    
    void Start()
    { 
        battlevol = battle.volume;
        dungvol = dungeon.volume;
        rewardVol = reward.volume;
        battle.Play();
        dungeon.Play();
        battle.DOFade(0,0);
    }

    public void ChangeToDungeon()
    {
        //makes battle go down
        battle.DOFade(0,2).OnComplete(() => dungeon.DOFade(dungvol,1));
        reward.DOFade(0,2);
    }

    public void EnterBattle()
    {   
        battle.Play();
        //makes dungeon go down
        reward.DOFade(0,1);
        dungeon.DOFade(0,.2f).OnComplete(() => battle.DOFade(battlevol,.2f));
    }

    public void Reward()
    {   
        reward.Play();
           reward.DOFade(rewardVol,.25f);
        //makes dungeon go down
        battle.DOFade(0,.25f);
        dungeon.DOFade(0,.25f);
    }

 
   
}
