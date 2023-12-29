using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : Singleton<MusicManager>
{
    public List<AudioSource> sources = new List<AudioSource>();
    Queue<AudioSource> q = new Queue<AudioSource>();
    AudioSource currentSource;

   protected override void Awake(){
        base.Awake();
        currentSource = GetSource();
    }

    public void ChangeMusic(SoundData newSong)
    {   
        if(newSong.audioClip == null){
            Debug.LogAssertion("NO CLIP FOUND!!");
            return;
        }
        currentSource.DOFade(0,.1f).OnComplete(()=>{

            AudioSource newSource =  GetSource();
            newSource.clip = newSong.audioClip;
            newSource.pitch = newSong.pitchRange.x;
            newSource.volume = 0;
            newSource.Play();
            newSource.DOFade( newSong.volume,.1f).OnComplete(()=>{
                currentSource.Stop();
                currentSource = newSource;
            });
           

        });
    }

    public void Silent(){
        if(currentSource != null){
            currentSource.DOFade(0,.1F);
        }
    }

    public AudioSource GetSource()
    {
        if(q.Count == 0)
        {
            q.Clear();

            foreach (var item in sources)
            {
                item.clip = null;
                item.pitch = 1;
                item.volume = 1;
                q.Enqueue(item);
            }
            return GetSource();

        }
        else
        {return q.Dequeue();}

     
       
    }

 
   
}
