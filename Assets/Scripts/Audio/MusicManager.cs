using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] Song dungeon;
    [SerializeField] Song battle;

    Song currentSong;
    [SerializeField] AudioSource introSource;
    [SerializeField] AudioSource mainSource;

    void Start()
    { Set(dungeon);}

    public void ChangeToDungeon(){
 Set(dungeon);
    }

    public void ChangeToBattle()
    {Set(battle);}

    public void Set(Song s)
    { 
        introSource.Stop();
        mainSource.Stop();
        if(s.intro != null)
        {
            introSource.volume = s.volume;
            mainSource.volume = s.volume;
            introSource.clip = s.intro;
            mainSource.clip = s.main;
          
            introSource.loop = false;
            mainSource.loop = true;
             
             
            StartCoroutine(PlayAfterDelay(s.intro.length));
            introSource.Play();
           
        }
        else
        {
            mainSource.volume = s.volume;
            mainSource.clip = s.main;
            mainSource.loop = true;
            mainSource.Play();
        }
        currentSong = s;
    }

    IEnumerator PlayAfterDelay(float delay)
    {
        
        yield return new WaitForSecondsRealtime(delay);
        mainSource.Play();
    }

   
}
