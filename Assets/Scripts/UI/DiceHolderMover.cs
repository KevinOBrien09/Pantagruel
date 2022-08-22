using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum DieMoveType{Attack,Heal}
public class DiceHolderMover : MonoBehaviour
{
    RectTransform rt;
    Vector2 ogPos;
    [SerializeField] float attackTarget;

    void Start()
    {

        rt = GetComponent<RectTransform>();
        ogPos = rt.anchoredPosition;
    }

    public void Move(DieMoveType dieMoveType,SoundData soundData,CastData cd,int change)
    {
        switch(dieMoveType)
        {
            case DieMoveType.Attack:
            StartCoroutine(Attack(soundData,cd,change));
            break;

            case DieMoveType.Heal:
            break;

        }
    }

    IEnumerator Attack(SoundData soundData,CastData castData,int dmg)
    {
        rt.DOAnchorPosX(0,.05f);
        yield return new WaitForSeconds(.075f);
        rt.DOAnchorPosX(attackTarget,.05f);
        yield return new WaitForSeconds(.05f);
        EffectManager.inst.Play(castData.effectGraphic);
        castData.target.OnHit(dmg);
        AudioManager.inst.GetSoundEffect().Play(soundData);
        rt.DOAnchorPosX(ogPos.x,.15f);
        yield return new WaitForSeconds(.3f);

    }


}