using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName ="Skills/BasicHeal")]
public class HealSkill : Skill
{
    public int baseHeal;
    
    public override float Go(CastData castData)
    {
        base.Go(castData);
        int mainValue = base.GetMainValue(castData);
        switch (skillData.miniGame)
        {
            case SkillMiniGame.Basic:
            castData.caster.OnHeal(mainValue);
            EffectManager.inst.Play(castData.effectGraphic);
            AudioManager.inst.GetSoundEffect().Play(skillData.sound);
            break;
        }
        return delayTilNextTurn;

    }
}
    