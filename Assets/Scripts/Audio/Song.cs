using UnityEngine;


[CreateAssetMenu(fileName = "Song Set")]
public class Song : ScriptableObject
{
   public AudioClip intro;
   public AudioClip main;
   public int pitch;
   public float volume;
}