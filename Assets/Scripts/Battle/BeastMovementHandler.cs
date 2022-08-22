using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeastMovementHandler
{
    public BeastGraphic graphic;
    public Transform transform;
    public void Init(BeastGraphic g ,Transform t , Vector3 p )
    {
        graphic = g;
        transform = t;
    }
    public IEnumerator Pounce(SoundData sfx,Skill skill,CastData cd)
    {
        graphic.SpringBoneOnOff(false);
        transform.DOLocalMoveZ(transform.position.z +2, .1f).OnComplete(()=> transform.DOMoveZ(transform.position.z -7, .05f).OnComplete( ()=>  
        {AudioManager.inst.GetSoundEffect().Play(sfx) ;  skill.Go(cd);}));
        yield return new WaitForSeconds(DamageSplash.shakeTime/2.5f);
        transform.DOLocalMoveZ(0, .1f).OnComplete( () => graphic.SpringBoneOnOff(true));
    }

    public IEnumerator Bounce(SoundData sfx,Skill skill,CastData cd)
    {
        transform.DOLocalMoveY(1, .1f).OnComplete( ()=>  
        {AudioManager.inst.GetSoundEffect().Play(sfx) ;  skill.Go(cd);});
        yield return new WaitForSeconds(.2f);
        transform.DOLocalMoveY(0, .1f);
    }

    public IEnumerator Spin(SoundData sfx,Skill skill,CastData cd)
    {
        
        transform.DOLocalRotate(new Vector3(0,-60,0),.2f);
        yield return new WaitForSeconds(.2f);
        transform.DOLocalRotate(new Vector3(0,40,0),.05f).OnComplete(()=> 
        {AudioManager.inst.GetSoundEffect().Play(sfx) ;  skill.Go(cd);});
        yield return new WaitForSeconds(.1f);
        transform.DOLocalRotate(new Vector3(0,0,0),.1f);

    }
    
}