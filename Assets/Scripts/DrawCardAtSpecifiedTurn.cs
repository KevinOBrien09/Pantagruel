using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/DrawCardAtSpecifiedTurn", fileName = "DrawCardAtSpecifiedTurn")]
public class DrawCardAtSpecifiedTurn : CardChoserEffect
{
    public int howManyCardsToChoseFrom;
    public int inHowManyTurns;
    public SoundData sfxSummon,sfxChoose;
    public override void Selected(Card card)
    {
        CardManager.inst.AddPredeterminedDraw(card, this);
        AudioManager.inst.GetSoundEffect().Play(sfxChoose);

    }

    public override bool canUse(bool isPlayer){return true;}
}