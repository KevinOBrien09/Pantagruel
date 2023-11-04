using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleEffectManager:Singleton<BattleEffectManager>                   
{
    public Animator animator;
    public GameObject lanceGO;
    public void Play(string vfx)
    {
      
        
        animator.Play(vfx);
    }
}