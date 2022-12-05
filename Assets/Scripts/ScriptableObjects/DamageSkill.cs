using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName ="Skills/BasicDamage")]
public class DamageSkill : Skill
{
    [Header("Reward")]
    public MiniGameReward reward;
    public int flatBonus;
    public int percentBonus;
    public StatName percentOfWhatStat;
    public List<StatusEffectData> rewardEffect = new List<StatusEffectData>();
    public Target rewardEffectTarget;
    public int rewardHowManyTurns;

    [Header("Coin")]
    public CoinResult desiredResult;

    [Header("Base Status Effect")]
    public bool applyEffect;
    public Target effectTarget;
    public int percentChance;
    public List<StatusEffectData> nonRewardEffect = new List<StatusEffectData>();
    public int baseHowManyTurns;
    
    public override float Go(CastData castData)
    {
        base.Go(castData);
        int mainValue = base.GetMainValue(castData);
 MainCameraManager.inst.SmallShake();
        switch (skillData.miniGame)
        {
            case SkillMiniGame.Basic:
            castData.target.OnHit(mainValue);
            if(applyEffect)
            {
                if(Maths.PercentCalculator(percentChance))
                {
                    if(effectTarget == Target.Opposition)
                    {castData.target.statusEffectHandler.ApplyStatusEffect(
                    StatusEffectFactory.Create(skillData.picture, baseHowManyTurns,nonRewardEffect),skillData.skillName);}
                    else{castData.caster.statusEffectHandler.ApplyStatusEffect(
                    StatusEffectFactory.Create(skillData.picture, baseHowManyTurns,nonRewardEffect),skillData.skillName);}
                }
            }
            break;

            case SkillMiniGame.Dice_DigitalRoot:
            int i =  DiceManager.inst.DamageAmount(castData.caster,castData.casterDR);
            if(castData.diceMover != null)
            {castData.diceMover.Move(DieMoveType.Attack,skillData.sound,castData,i);}
            else
            {castData.target.OnHit(i);}
             if(applyEffect)
            {
                if(Maths.PercentCalculator(percentChance))
                {
                    if(effectTarget == Target.Opposition)
                    {castData.target.statusEffectHandler.ApplyStatusEffect(
                    StatusEffectFactory.Create(skillData.picture, baseHowManyTurns,nonRewardEffect),skillData.skillName);}
                    else{castData.caster.statusEffectHandler.ApplyStatusEffect(
                    StatusEffectFactory.Create(skillData.picture, baseHowManyTurns,nonRewardEffect),skillData.skillName);}
                }
            }
            break;

            case SkillMiniGame.Coin_HeadTails:
            
            if(CoinManager.coinResult != desiredResult)
            {castData.target.OnHit(mainValue);}
            else
            {Reward();
            castData.target.OnHit(mainValue);}
            
            break;
            
            case SkillMiniGame.Gauge:
            if(GaugeTicker.inst.sweetSpot)
            {
                Debug.Log("hehe");
                //castData.target.OnHit(mainValue);
                Reward();
            }
            else
            {castData.target.OnHit(mainValue);}

            break;
            
            default:
            Debug.LogAssertion("Default Switch");
            break;
        }

        return delayTilNextTurn;


        void Reward()
        {
            if(reward == MiniGameReward.FlatBonus)
            {castData.target.OnHit(mainValue + flatBonus);}
            else if(reward == MiniGameReward.PercentBonus)
            {
                int baseStat = castData.caster.GetModifiedStat(percentOfWhatStat);
                float calculation = (float)Maths.Percent(baseStat,percentBonus);
                //calculation = Mathf.Clamp(calculation,-90,90);
                castData.target.OnHit(mainValue + (int)calculation);
            }
            else if(reward == MiniGameReward.StatusEffect)
            {  
                if(rewardEffectTarget == Target.Opposition){
                castData.target.statusEffectHandler.ApplyStatusEffect(
                StatusEffectFactory.Create(skillData.picture,rewardHowManyTurns,rewardEffect),skillData.skillName);
                }else{
                castData.caster.statusEffectHandler.ApplyStatusEffect(
                StatusEffectFactory.Create(skillData.picture,rewardHowManyTurns,rewardEffect),skillData.skillName);
                }
             
            }
        
        }
    }

    


   
}
