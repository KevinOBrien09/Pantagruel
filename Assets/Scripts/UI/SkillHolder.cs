using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillHolder : MonoBehaviour
{
    public SkillData data;
    public Image picture;
    public SkillButton button;
    public SoundData hover;


    public void Apply(Skill s)
    {
        data = new SkillData(s.skillData);
        picture.sprite = data.picture;
        button.hover = hover;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {BattleManager.inst.caster.Precast(s);});
    }


}