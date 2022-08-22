using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public struct SkillResource
{
    public ResourceCurrency resource;
    public int amount;

    public SkillResource(SkillResource sd)
    {
        resource = sd.resource;
        amount = sd.amount;
    }
}

public class Skill:ScriptableObject
{
    public int baseChange;
    public SkillData skillData;
    public float delayTilNextTurn = 1f;

 
    public virtual float Go(CastData castData)
    {
        EffectManager.inst.Play(castData.effectGraphic);
        AudioManager.inst.GetSoundEffect().Play(skillData.sound);

        

        return delayTilNextTurn;
    }
    public int GetMainValue(CastData castData)
    {
        int phys = 0;
        int magic = 0;
        int tough = 0;
        int res = 0;
        int chr = 0;
        
        if(skillData.skillDesc.Contains(StatTags.PHYS))
        {
            (int,string) sneed = MiscFunctions.GetPercentFromString(skillData.skillDesc,StatTags.PHYS);
            double p = Maths.Percent(castData.caster.data.stats.physical,sneed.Item1);
            phys = (int)p;
        }
        
        if(skillData.skillDesc.Contains(StatTags.MGK))
        {
            (int,string) sneed = MiscFunctions.GetPercentFromString(skillData.skillDesc,StatTags.MGK);
            double p = Maths.Percent(castData.caster.data.stats.magic,sneed.Item1);
            magic = (int)p;
        }
        
        if(skillData.skillDesc.Contains(StatTags.TGH))
        {
            (int,string) sneed = MiscFunctions.GetPercentFromString(skillData.skillDesc,StatTags.TGH);
            double p = Maths.Percent(castData.caster.data.stats.toughness,sneed.Item1);
            tough = (int)p;
        }
        
        if(skillData.skillDesc.Contains(StatTags.RES))
        {
            (int,string) sneed = MiscFunctions.GetPercentFromString(skillData.skillDesc,StatTags.RES);
            double p = Maths.Percent(castData.caster.data.stats.resolve,sneed.Item1);
            res = (int)p;
        }
        
        if(skillData.skillDesc.Contains(StatTags.CHR))
        {
            (int,string) sneed = MiscFunctions.GetPercentFromString(skillData.skillDesc,StatTags.CHR);
            double p = Maths.Percent(castData.caster.data.stats.charisma,sneed.Item1);
            chr = (int)p;
        }
       
        return baseChange + phys + magic + tough + res + chr;
    }
}

[System.Serializable]
public class SkillData
{
    public string skillName;
    [TextArea] public string skillDesc;
    public Sprite picture;
    public Target target;
    public BeastMoveType beastMoveType;
    public SoundData sound;
    public SkillMiniGame miniGame;
    public List<int> gaugeBullseyes = new List<int>();
    public float gaugeSpeed;
    public SkillResource cost;
    public EffectGraphic effectGraphic;
    public bool cantCastWithFullHealth;
    

    public SkillData(SkillData s)
    {
        if(s!= null)
        {
            skillName = s.skillName;
            skillDesc = s.skillDesc;
            picture = s.picture;
            target = s.target;
            beastMoveType = s.beastMoveType;
            miniGame = s.miniGame;
            gaugeBullseyes = new List<int>(s.gaugeBullseyes);
            gaugeSpeed = s.gaugeSpeed;
            cost = new SkillResource(s.cost);
            effectGraphic = s.effectGraphic;
            cantCastWithFullHealth = s.cantCastWithFullHealth;
           
        }
    }
}

[System.Serializable]
public class CastData
{
    public Beast caster;
    public Beast target;
    public int casterDR;
    public int targetDR;
    public DiceHolderMover diceMover;
    public EffectGraphic effectGraphic;

    public CastData
    (
        CastData data = null,
        Beast caster_ = null,
        Beast target_ = null,
        int casterDigitalRoot=0,
        int targetDigitalRoot = 0,
        DiceHolderMover diceMover_ = null,
        EffectGraphic effectGraphic_ = EffectGraphic.None
    )
    {
        if(data != null)
        {
            caster = data.caster;
            target = data.target;
            casterDR = data.casterDR;
            targetDR = data.targetDR;
            diceMover = data.diceMover;
            effectGraphic = data.effectGraphic;

        }
        else
        {
            if(caster_ == null)
            {Debug.LogAssertion("No caster");return;}
            caster = caster_;
            target = target_;
            casterDR = casterDigitalRoot;
            targetDR = targetDigitalRoot;
            diceMover = diceMover_;
            effectGraphic = effectGraphic_;


        }

    }
    

}
