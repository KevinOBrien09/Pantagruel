using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using System.Linq;

public class BeastEditor : DragHandler
{
 
   public TMP_Dropdown elementDropdown;
    public TMP_Dropdown classDropdown;
     public TMP_Dropdown secondaryClassDropdown;
    public BeastScriptableObject beastScriptableObject;
    public TextMeshProUGUI beastName;
    public Transform countHolder,classCount,secondaryClassCount;
    public TextMeshProUGUI Counter;
    List<BeastScriptableObject> modfiedObjects = new List<BeastScriptableObject>();
    public List<StatEditor> baseStats = new List<StatEditor>();
     public List<StatEditor> statGrowth = new List<StatEditor>();
     public TextMeshProUGUI baseStatTotal,statGrowthTotal;
     public TMP_InputField devNotes;
    public Book book;
    public override void  Start()
    {
        base.Start();
        var elementEnums = Enum.GetValues(typeof(Element));
        List<string> elements = new List<string>();
        foreach (Element item in elementEnums )
        {elements.Add(item.ToString());}
        elementDropdown.options.Clear();
        foreach (var item in elements)
        {elementDropdown.options.Add(new TMP_Dropdown.OptionData(item));}


        var classEnums = Enum.GetValues(typeof(BeastClass));
        List<string> classes = new List<string>();
        foreach (BeastClass item in classEnums )
        {classes.Add(item.ToString());}
        classDropdown.options.Clear();
        secondaryClassDropdown.options.Clear();
        foreach (var item in classes)
        {classDropdown.options.Add(new TMP_Dropdown.OptionData(item));
        secondaryClassDropdown.options.Add(new TMP_Dropdown.OptionData(item));
        }

       Rankings();
        CreateCounters();

    }

    public void ToggleOnOff(){
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void ChangeBeast(BeastScriptableObject bso){
        beastScriptableObject = bso;
        beastName.text = "EDITING :" +beastScriptableObject.beastData.beastName;
        elementDropdown.value = (int) beastScriptableObject.beastData.element;
        classDropdown.value = (int)beastScriptableObject.beastData.beastClass;
        secondaryClassDropdown.value = (int)beastScriptableObject.beastData.secondaryClass;
     
        foreach (var item in baseStats)
        {
            Stats s =  beastScriptableObject.beastData.stats;
            switch (item.statName)
            {
                case StatName.HEALTH:
                item.inputField.text = s.maxHealth.ToString();
                break;
                case StatName.DEFENSE:
                item.inputField.text = s.def.ToString();
                break;
                case StatName.PHYSICAL:
                item.inputField.text = s.physical.ToString();
                break;
                case StatName.MAGIC:
                item.inputField.text = s.magic.ToString();
                break;
                case StatName.CHARISMA:
                item.inputField.text = s.charisma.ToString();
                break;
                case StatName.LUCK:
                item.inputField.text = s.luck.ToString();
                break;
            }
        }
        
        foreach (var item in statGrowth)
        {
            Stats s =  beastScriptableObject.beastData.statGrowthPerLevel;
            switch (item.statName)
            {
                case StatName.HEALTH:
                item.inputField.text = s.maxHealth.ToString();
                break;
                case StatName.DEFENSE:
                item.inputField.text = s.def.ToString();
                break;
                case StatName.PHYSICAL:
                item.inputField.text = s.physical.ToString();
                break;
                case StatName.MAGIC:
                item.inputField.text = s.magic.ToString();
                break;
                case StatName.CHARISMA:
                item.inputField.text = s.charisma.ToString();
                break;
                case StatName.LUCK:
                item.inputField.text = s.luck.ToString();
                break;
            }
        }
        devNotes.text = beastScriptableObject.beastData.devNote.note;
        UpdateStatTotals();
       
    }

    public void UpdateStatTotals()
    {
        int baseStatTotalint = 0;
        int statGrowthTotalint = 0;

        foreach (var item in baseStats)
        {baseStatTotalint += int.Parse(item.inputField.text.ToString());}
        
        foreach (var item in statGrowth)
        {statGrowthTotalint += int.Parse(item.inputField.text.ToString()) ;}

        baseStatTotal.text = "TOTAL: " + baseStatTotalint.ToString();
        statGrowthTotal.text = "TOTAL: " + statGrowthTotalint.ToString();
    }

    public void ApplyNewBeastInfo()
    {
        beastScriptableObject.beastData.element = (Element)elementDropdown.value;
        beastScriptableObject.beastData.beastClass = (BeastClass)classDropdown.value;
        beastScriptableObject.beastData.secondaryClass = (BeastClass)secondaryClassDropdown.value;
        beastScriptableObject.beastData.devNote.note = devNotes.text;
        
        foreach (var item in baseStats)
        {
            
            switch (item.statName)
            {
                case StatName.HEALTH:
                beastScriptableObject.beastData.stats.maxHealth = int.Parse(item.inputField.text.ToString());
                break;
                case StatName.DEFENSE:
                beastScriptableObject.beastData.stats.def = int.Parse(item.inputField.text.ToString());
                break;
                case StatName.PHYSICAL:
                beastScriptableObject.beastData.stats.physical = int.Parse(item.inputField.text.ToString());
                break;
                case StatName.MAGIC:
                beastScriptableObject.beastData.stats.magic = int.Parse(item.inputField.text.ToString());
                break;
                case StatName.CHARISMA:
                beastScriptableObject.beastData.stats.charisma = int.Parse(item.inputField.text.ToString());
                break;
                case StatName.LUCK:
                beastScriptableObject.beastData.stats.luck = int.Parse(item.inputField.text.ToString());
                break;
            }
        }


        foreach (var item in statGrowth)
        {
            
            switch (item.statName)
            {
                case StatName.HEALTH:
                beastScriptableObject.beastData.statGrowthPerLevel.maxHealth = int.Parse(item.inputField.text.ToString());
                break;
                case StatName.DEFENSE:
                beastScriptableObject.beastData.statGrowthPerLevel.def = int.Parse(item.inputField.text.ToString());
                break;
                case StatName.PHYSICAL:
                beastScriptableObject.beastData.statGrowthPerLevel.physical = int.Parse(item.inputField.text.ToString());
                break;
                case StatName.MAGIC:
                beastScriptableObject.beastData.statGrowthPerLevel.magic = int.Parse(item.inputField.text.ToString());
                break;
                case StatName.CHARISMA:
                beastScriptableObject.beastData.statGrowthPerLevel.charisma = int.Parse(item.inputField.text.ToString());
                break;
                case StatName.LUCK:
                beastScriptableObject.beastData.statGrowthPerLevel.luck = int.Parse(item.inputField.text.ToString());
                break;
            }
        }
       
        modfiedObjects.Add(beastScriptableObject);
        CreateCounters();
       
    }

   public void CreateCounters()
   {
        foreach (Transform item in countHolder)
        {
            Destroy(item.gameObject);
        }

        foreach (Transform item in classCount)
        {
            Destroy(item.gameObject);
        }

         foreach (Transform item in secondaryClassCount)
        {
            Destroy(item.gameObject);
        }


        Dictionary<Element,List<BeastScriptableObject>> dict = new Dictionary<Element, List<BeastScriptableObject>>();
        Dictionary<BeastClass,List<BeastScriptableObject>> classdict = new Dictionary<BeastClass, List<BeastScriptableObject>>();
         Dictionary<BeastClass,List<BeastScriptableObject>> secondaryclassdict = new Dictionary<BeastClass, List<BeastScriptableObject>>();
        foreach (Element el in Enum.GetValues(typeof(Element)))
        {dict.Add(el,new List<BeastScriptableObject>());}
        foreach (BeastClass b in Enum.GetValues(typeof(BeastClass)))
        {
            classdict.Add(b,new List<BeastScriptableObject>());
            secondaryclassdict.Add(b,new List<BeastScriptableObject>());
        }

        foreach (var item in book.beastScriptableObjects)
        {
            // if((int)item.beastData.element == 6 | (int)item.beastData.element == 7){
            //     Debug.Log(item.beastData.beastName);
            //     item.beastData.element =
            //     continue;
            // }
            dict[item.beastData.element].Add(item);
            classdict[item.beastData.beastClass].Add(item);
            secondaryclassdict[item.beastData.secondaryClass].Add(item);
            string s = item.beastData.element.ToString()+ " " +  item.beastData.beastClass.ToString();
            if(item.beastData.secondaryClass != BeastClass.COMMON){
                s+= item.beastData.secondaryClass.ToString();
            }
          //  Debug.Log(s);
        }

        foreach (var item in dict)
        {
            TextMeshProUGUI t = Instantiate(Counter,countHolder);
         
            t.gameObject.SetActive(true);
        
            t.text = item.Key.ToString() + ":" + item.Value.Count;       
               
        }

      
        foreach (var item in classdict)
        {
            TextMeshProUGUI t = Instantiate(Counter,classCount);
            t.gameObject.SetActive(true);
            t.text = item.Key.ToString() + ":" + item.Value.Count + " ";       
       
        }

         foreach (var item in secondaryclassdict)
        {
            TextMeshProUGUI t = Instantiate(Counter,secondaryClassCount);
            t.gameObject.SetActive(true);
            t.text = item.Key.ToString() + ":" + item.Value.Count + " ";       
       
        }
    }


    public (Dictionary<BeastScriptableObject,int> baseStats,Dictionary<BeastScriptableObject,int> growth) Rankings()
    {

        Dictionary<BeastScriptableObject,int> statTotal = new Dictionary<BeastScriptableObject, int>();
        Dictionary<BeastScriptableObject,int> statGrowthTotal = new Dictionary<BeastScriptableObject, int>();
        foreach (var item in book.beastScriptableObjects)
        {
          
            statTotal.Add(item,item.beastData.stats.GetStatTotal());
            statGrowthTotal.Add(item,item.beastData.statGrowthPerLevel.GetStatTotal());
        }

        var baseOrdered = statTotal.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        var growthOrdered = statGrowthTotal.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        Debug.Log(baseOrdered.First().Key.beastData.beastName + " has the lowest base stats");
        Debug.Log(baseOrdered.Last().Key.beastData.beastName + " has the highest base stats");
        Debug.Log(growthOrdered.First().Key.beastData.beastName + " has the lowest stat growth");
        Debug.Log(growthOrdered.Last().Key.beastData.beastName + " has the highest stat growth");

        int q = 0;
        foreach (var item in baseOrdered)
        {
            q++;
            Debug.Log(item.Key.beastData.beastName + " is placed " + q + " with a base stat total of " + item.Value);
        }
        q = 0;
        // foreach (var item in growthOrdered)
        // {
        //     q++;
        //     Debug.Log(item.Key.beastData.beastName + " is placed " + q + " with a stat growth total of " + item.Value);
        // }

        return (baseOrdered,growthOrdered);


    }
   #if UNITY_EDITOR
    void OnDestroy(){
        foreach (var item in modfiedObjects)
        {
              EditorUtility.SetDirty(item);
        }

        AssetDatabase.SaveAssets();

    }

   #endif
}
