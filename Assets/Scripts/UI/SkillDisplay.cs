using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SkillDisplay : Singleton<SkillDisplay>
{   

    public enum UpdateStyle{DR,Stats}

    [SerializeField] Image bg;
    [SerializeField] TextMeshProUGUI skillName;
    [SerializeField] TextMeshProUGUI skillDesc;
    [SerializeField] TextMeshProUGUI skillCost;
    const float fadeSpeed = .3f;
    List<Tween> t = new List<Tween>();
    public Coroutine c;
    public string currentDesc;


    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
        skillName.DOFade(0,fadeSpeed);
        skillCost.DOFade(0,fadeSpeed);
        skillDesc.DOFade(0,fadeSpeed);

    }
    
    
    public void Show(SkillData s)
    {
        if(c!= null)
        {
            StopCoroutine(c);
            foreach (var item in t)
            {item.Kill();}
            c=null;
        }

        gameObject.SetActive(true);
        skillName.text = s.skillName;
      
        switch (s.cost.resource)
        {
            case ResourceCurrency.SP:
            skillCost.text = s.cost.amount.ToString() + ":SP";
            break;
            case ResourceCurrency.HP:
            skillCost.text = s.cost.amount.ToString() + ":<color=red>HP</color>";
            break;

            
            default:
            Debug.LogAssertion("Default Switch");
            break;
        }
      
        SetDesc(s);
        bg.DOFade(1,fadeSpeed);
        skillName.DOFade(1,fadeSpeed);
        skillDesc.DOFade(1,fadeSpeed);
        skillCost.DOFade(1,fadeSpeed);
    }

    public void SetDesc(SkillData s,string custom = "")
    {
        if(s!= null)
        {
            currentDesc = s.skillDesc;
            skillDesc.text = currentDesc;
        }
        else
        {
            currentDesc = custom;
            skillDesc.text = custom;
        }

        ParseDesc(sd:s);
    }

    public void ParseDesc(SkillData sd = null)
    {
        string s = currentDesc;

        if(s.Contains("<dr>"))
        {
            if(!string.IsNullOrWhiteSpace(BattleManager.inst.miniGameResultHandler.display))
            {s = s.Replace("<dr>",BattleManager.inst.miniGameResultHandler.display);}
            else
            {s = s.Replace("<dr>","(DR)");}
        }

        if(s.Contains(StatTags.PHYS))
        {
            //bug
            (int,string)data = MiscFunctions.GetPercentFromString(s,StatTags.PHYS);
            int amount = (int)Maths.Percent(BattleManager.inst.activePlayerBeast.GetModifiedStat(StatName.Physical),data.Item1);
            string d = "<color=red>"+amount.ToString()+"</color>";
            s = s.Replace(data.Item2,d);
        }

        if(s.Contains(StatTags.MGK))
        {
            //bug
            (int,string)data = MiscFunctions.GetPercentFromString(s,StatTags.MGK);
            int amount = (int)Maths.Percent(BattleManager.inst.activePlayerBeast.GetModifiedStat(StatName.Magic),data.Item1);
            string d = "<color=red>"+amount.ToString()+"</color>";
            s = s.Replace(data.Item2,d);
        }

        if(s.Contains(StatTags.TGH))
        {
            //bug
            (int,string)data = MiscFunctions.GetPercentFromString(s,StatTags.TGH);
            int amount = (int)Maths.Percent(BattleManager.inst.activePlayerBeast.GetModifiedStat(StatName.Toughness),data.Item1);
              string d = "<color=red>"+amount.ToString()+"</color>";
            s = s.Replace(data.Item2,d);
        }

        if(s.Contains(StatTags.RES))
        {
            //bug
            (int,string)data = MiscFunctions.GetPercentFromString(s,StatTags.RES);
            int amount = (int)Maths.Percent(BattleManager.inst.activePlayerBeast.GetModifiedStat(StatName.Resolve),data.Item1);
             string d = "<color=red>"+amount.ToString()+"</color>";
            s = s.Replace(data.Item2,d);
        }

        if(s.Contains(StatTags.CHR))
        {
            //bug
            (int,string)data = MiscFunctions.GetPercentFromString(s,StatTags.CHR);
            int amount = (int)Maths.Percent(BattleManager.inst.activePlayerBeast.GetModifiedStat(StatName.Charisma),data.Item1);
            string d = "<color=red>"+amount.ToString()+"</color>";
            s = s.Replace(data.Item2,d);
        }
        skillDesc.text = s;
    }

    public IEnumerator Hide()
    {   
        yield return null;
        yield return new WaitForSeconds(.15f);
        yield return null;
        t.Add(bg.DOFade(0,fadeSpeed)) ;
        t.Add(skillName.DOFade(0,fadeSpeed));
        t.Add(skillCost.DOFade(0,fadeSpeed));
        t.Add(skillDesc.DOFade(0,fadeSpeed).OnComplete( () => {gameObject.SetActive(false);} ));
    }



}