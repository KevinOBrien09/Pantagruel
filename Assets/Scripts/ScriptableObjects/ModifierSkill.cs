using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffDebuff{Buff,Debuff}
[CreateAssetMenu(fileName = "Skill", menuName ="Skills/BasicModifier")]
public class ModifierSkill : Skill
{
    public List<StatusEffectData> changeData = new List<StatusEffectData>();
    public int howManyTurns;

    public List<StatusEffectData> miniGameData = new List<StatusEffectData>();
    public bool applyBoth;

    [SerializeField] CoinResult coinResult;
    public override float Go(CastData castData)
    {
        base.Go(castData);
        switch (skillData.miniGame)
        {
            case SkillMiniGame.Basic:
       
            castData.target.statusEffectHandler.ApplyStatusEffect(
            StatusEffectFactory.Create(skillData.picture,howManyTurns,changeData),skillData.skillName);
            
            break;

            case SkillMiniGame.Gauge:
            if(GaugeTicker.inst.sweetSpot)
            {
                if(applyBoth)
                {
                    List<StatusEffectData> sed = new List<StatusEffectData>();

                    foreach (var item in changeData)
                    {
                        sed.Add(item);
                    }
                    
                    foreach (var item in miniGameData)
                    {
                        sed.Add(item);
                    }

                    if(changeData.Count > 0)
                    {
                        castData.target.statusEffectHandler.ApplyStatusEffect(
                        StatusEffectFactory.Create(skillData.picture,howManyTurns,sed),skillData.skillName);
                    }

                }
                else
                {
                    if(miniGameData.Count > 0)
                    {
                        castData.target.statusEffectHandler.ApplyStatusEffect(
                        StatusEffectFactory.Create(skillData.picture,howManyTurns,miniGameData),skillData.skillName);
                    }

                }
            }
            else
            {
                if(changeData.Count > 0){
                castData.target.statusEffectHandler.ApplyStatusEffect(
                StatusEffectFactory.Create(skillData.picture,howManyTurns,changeData),skillData.skillName);
                }
            }

            break;

            case SkillMiniGame.Coin_HeadTails:
            if(CoinManager.coinResult == coinResult)
            {
             
                if(applyBoth)
                {
                    List<StatusEffectData> sed = new List<StatusEffectData>();

                    foreach (var item in changeData)
                    {
                        sed.Add(item);
                    }
                    
                    foreach (var item in miniGameData)
                    {
                        sed.Add(item);
                    }

                    if(changeData.Count > 0)
                    {
                        castData.target.statusEffectHandler.ApplyStatusEffect(
                        StatusEffectFactory.Create(skillData.picture,howManyTurns,sed),skillData.skillName);
                    }

                }
                else{
   if(miniGameData.Count > 0)
                {
                    castData.target.statusEffectHandler.ApplyStatusEffect(
                    StatusEffectFactory.Create(skillData.picture,howManyTurns,miniGameData),skillData.skillName);
                }

                }
               
            }
            else
            {
                if(changeData.Count > 0)
                {
                    castData.target.statusEffectHandler.ApplyStatusEffect(
                    StatusEffectFactory.Create(skillData.picture,howManyTurns,changeData),skillData.skillName);
                }
            }


            break;
        }
        return delayTilNextTurn;
    }
}

