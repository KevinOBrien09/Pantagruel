using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainTextTicker : Singleton<MainTextTicker>
{
    public TMP_Typewriter typewriter;
    [SerializeField] float textSpeed;
    public void Type(string s)
    {typewriter.Play(s,textSpeed,() => {});}
   
}
