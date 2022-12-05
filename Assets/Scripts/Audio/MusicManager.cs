using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] AudioSource dungeon;
    [SerializeField]  AudioSource battle;
    float battlevol,dungvol;
    Song currentSong;
    

    void Start()
    { 
        battlevol = battle.volume;
        dungvol = dungeon.volume;
        battle.Play();
        dungeon.Play();
        
      battle.DOFade(0,0);
    }

    public void ChangeToDungeon()
    {

        battle.DOFade(0,1).OnComplete(() => dungeon.DOFade(dungvol,1));
    }

    public void ChangeToBattle()
    {   
        battle.Play();
       dungeon.DOFade(0,.2f).OnComplete(() => battle.DOFade(battlevol,.2f));
    }

   

 
   
}
