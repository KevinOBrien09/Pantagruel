using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GenericSounds : Singleton<GenericSounds>
{
    public AudioSource buff;
    public AudioSource deBuff;
    public AudioSource heal;

    public void HealSFX()
    {
        heal.pitch = Random.Range(.9f,1.1f);
        heal.Play();
    }

    public void BuffSFX()
    {
        buff.pitch = Random.Range(.9f,1.1f);
        buff.Play();
    }

    public void DebuffSFX()
    {
        deBuff.pitch = Random.Range(.9f,1.1f);
        deBuff.Play();
    }


}