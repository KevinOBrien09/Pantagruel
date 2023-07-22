using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasySpringBone;
using System.Linq;

public class BeastAnimatedInstance : MonoBehaviour
{
    public List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    public List<SpringBone> springBones = new List<SpringBone>();
    Animator animator;

    public void Init(Beast b)
    {
        SpriteRenderer a = null;
        
        if(gameObject.TryGetComponent<SpriteRenderer>(out a))
        {renderers.Add(a);}
        
        springBones = gameObject.GetComponentsInChildren<SpringBone>().ToList();
        renderers = gameObject.GetComponentsInChildren<SpriteRenderer>().ToList();

        Animator an;
        if(TryGetComponent<Animator>(out an))
        {animator = an;}

        b.animatedInstance = this;
    }

    public void TakeDamage()
    {
        Vector3 p = transform.position;

        foreach (var item in springBones)
        {item.ignoreSpringBone = true;}
if(animator != null){
 animator.speed = 0;
}
       

        transform.DOShakePosition(.5f,.1f,10,90,false,true).OnComplete(()=>
        {
            transform.DOMove(p,0);
            foreach (var item in springBones)
            {item.ignoreSpringBone = true;}
        });

        foreach (var item in renderers)
        {
            StartCoroutine(q());
            IEnumerator q(){
                item.DOColor(Color.red,0);
                yield return new WaitForSeconds(.5f);
                item.DOColor(Color.white,.2f);
                if(animator != null){
                animator.speed = 1;
                }
            }
        }
    }

}