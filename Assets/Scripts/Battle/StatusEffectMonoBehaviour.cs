using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic; 
using TMPro;
using System.Linq;

public class StatusEffectMonoBehaviour : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Image picture;
    public GameObject info;
    public List<StatusEffectInstance> statusEffectInstances = new List<StatusEffectInstance>();
    [SerializeField] TextMeshProUGUI mainInfo;
    [SerializeField] TextMeshProUGUI[] stack;
    [SerializeField] RectTransform enemyInfoPos;
    
    void Start() => info.SetActive(false);

    public void Apply(StatusEffectInstance i,Alliance a)
    {
        picture.sprite = i.picture;

        if(a == Alliance.Enemy)
        {
           // info.GetComponent<RectTransform>().anchoredPosition = enemyInfoPos.anchoredPosition;
            info.transform.localRotation = Quaternion.Euler(-180,0,0);
            mainInfo.transform.localRotation = Quaternion.Euler(180,0,0);
        }
    }

    public bool RemoveInstance(StatusEffectInstance i)
    {
        StatusEffectInstance result = statusEffectInstances.Find(x => x == i);
        statusEffectInstances.Remove(result);
        result.Exit();

        if(statusEffectInstances.Count <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        else
        {
            UpdateInfoDisplay();
            return false;
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    public void UpdateInfoDisplay()
    {
        mainInfo.text = "";

        int turnsLeft = 0;
        foreach (var item in statusEffectInstances)
        {turnsLeft += item.howManyTurns;}
        
        Dictionary<StatName,int> statChanges = new Dictionary<StatName, int>();
        int bleed = 0;

        foreach (StatusEffectInstance ins in statusEffectInstances)
        {
            foreach (StatusEffect effect in ins.effects)
            {
                switch (effect)
                {
                    case StatModification sm:
                    StatModification s = (StatModification)effect;

                    if(statChanges.ContainsKey(s.statModData.statName))
                    {
                        statChanges[s.statModData.statName] += s.statModData.change;
                        if(s.statModData.statName == StatName.Toughness || s.statModData.statName == StatName.Resolve)
                        {statChanges[s.statModData.statName]  = Mathf.Clamp( statChanges[s.statModData.statName] ,-90,90);}
                         
                    }
                    else
                    {
                        statChanges.Add(s.statModData.statName,s.statModData.change);
                        if(s.statModData.statName == StatName.Toughness || s.statModData.statName == StatName.Resolve)
                        {statChanges[s.statModData.statName]  = Mathf.Clamp( statChanges[s.statModData.statName] ,-90,90);}
                    }
                    break;
                    
                    case Bleed bl:
                    Bleed b = (Bleed)effect;
                    bleed += b.bleedData.damagePerTurn;
                    break;
                    
                    default:
                    Debug.LogAssertion("DefaultSwitch");
                    break;
                }
            }
        }
        
        List<string> effects = new List<string>();
        if(statChanges.Count > 0)
        {
            foreach (var item in statChanges)
            {
                
                string s = "";
                if(item.Value > 0)
                {s = "+";}
                s+=item.Value.ToString() + " " + MiscFunctions.GetAbbStatName(item.Key);
                s+= "<br>";
                effects.Add(s);
            }
        }
        
        if(bleed > 0)
        {
            string b = bleed.ToString() + " bleed";
            b += "<br>";
            effects.Add(b);
        }
        
        string main = "";
        foreach (var item in effects)
        {main += item;}
        
        //main += turnsLeft.ToString() + " turns";
        mainInfo.text = main;
        
        foreach (var item in stack)
        { item.text = statusEffectInstances.Count.ToString(); }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        info.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      info.SetActive(false);
    }
}
