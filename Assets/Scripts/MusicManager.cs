using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : Singleton<MusicManager>
{
   public AudioSource dungeon;
    public  AudioSource battle;
public  AudioSource reward;
   public float battlevol,dungvol,rewardVol;
    float battleOg;
    void Start()
    { 
        battleOg = battle.volume;
        ResetVol();
        battle.Play();
        dungeon.Play();
        battle.DOFade(0,0);
    }

    public void ResetVol(){
        battlevol = battle.volume;
        dungvol = dungeon.volume;
        rewardVol = reward.volume;
    }

    public void ChangeBGMusicWithFade(SoundData newSong){
        dungeon.DOFade(0,.25f).OnComplete(()=>
        {
            StartCoroutine(q());
            IEnumerator q()
            {
                ChangeBGMusic(newSong);
                dungeon.DOFade(0,0);
                yield return new WaitForSeconds(.2f);
                dungeon.DOFade(dungvol,1);
            }

        });
    }

    public void ChangeBGMusic(SoundData newSong){
        dungeon.clip = newSong.audioClip;
        dungeon.volume = newSong.volume;
        dungeon.pitch = newSong.pitchRange.x;
        dungeon.Play();
        ResetVol();
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
         battle.DOFade(battleOg,.0f);
        dungeon.DOFade(0,.2f);
      //  .OnComplete(() =>);
    }

    public void EndBattleMusic(){
 battle.DOFade(0,1f);
    }

    public void Reward()
    {   
        reward.Play();
           reward.DOFade(rewardVol,.25f);
        //makes dungeon go down
       EndBattleMusic();
        dungeon.DOFade(0,.25f);
    }

 
   
}
