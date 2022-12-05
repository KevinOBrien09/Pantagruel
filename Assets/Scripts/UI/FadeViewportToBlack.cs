using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class FadeViewportToBlack:Singleton<FadeViewportToBlack>
{
    [SerializeField] Image image;
    void Start()=>image.DOFade(0,0);
    public void Fade(float time)
    {
        image.DOFade(1,time);
    }

    public void UnFade(float time)
    {
        image.DOFade(0,time);
    }

}