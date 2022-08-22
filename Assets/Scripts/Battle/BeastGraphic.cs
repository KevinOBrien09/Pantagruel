using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasySpringBone;

public class BeastGraphic : MonoBehaviour
{
    public List<SpriteRenderer> sprites;
    public List<SpringBone> springBones = new List<SpringBone>();
    [SerializeField] Wind wind;
    [SerializeField] Animator animator;


    public IEnumerator HitCoro(bool shake)
    {
        SpringBoneOnOff(false);
       
        //  yield return new WaitForSeconds(.16f);
        animator.speed = 0;
        foreach (var item in sprites)
        {item.color = Color.red; }
        if(shake)
        {transform.parent.DOShakePosition(.3f,.5f,20).OnComplete(()=> transform.parent.DOLocalMove(Vector3.zero,.1f));}
        yield return new WaitForSeconds(.4f);

        foreach (var item in sprites)
        {item.color = Color.white; }
        animator.speed = 1;
        SpringBoneOnOff(true);
    }

    public void SpringBoneOnOff(bool status)
    {

        if(!status)
        {
            wind?.removeExtraForce();
            foreach (var item in springBones)
            {
                item.ignoreSpringBone = true;
            }

        
        }
        else{
            wind?.ActivateWind();
            foreach (var item in springBones)
            {
                item.ignoreSpringBone = false;
            }

        
        }
    }

    public void Fade()
    {StartCoroutine(RealFade());
       
    }

    IEnumerator RealFade()
    {
        animator.speed = 0;
        yield return new WaitForSeconds(.75f);
       
      
        foreach (var item in sprites)
        {
            item.DOFade(0,1.5f);
        }
    }

    public void ChangeLayersToForefront()
    {
        foreach (var item in sprites)
        {
            item.sortingOrder = item.sortingOrder * 2;
        }

    }

    public void ChangeLayersToBack()
    {
        foreach (var item in sprites)
        {
            item.sortingOrder = item.sortingOrder / 2;
        }

    }



}