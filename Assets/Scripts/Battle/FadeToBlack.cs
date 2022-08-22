using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class FadeToBlack:Singleton<FadeToBlack>
{
    [SerializeField] Image image;
    void Start()=>image.DOFade(0,0);
    public void Fade(float time)
    {
        image.DOFade(1,time);
    }

}