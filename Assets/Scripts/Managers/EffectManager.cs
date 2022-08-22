using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [SerializeField] Animator animator;

    public void Play(EffectGraphic eg)
    {
        animator.Play(eg.ToString());
    }
    
}