using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    public int maxHealth;
    public int def;
    public int physical;
    public int magic;
    public int charisma;
    public int luck;

    
    public Stats(Stats s)
    {
        maxHealth = s.maxHealth;
        def = s.def;
        physical = s.physical;
        magic = s.magic;
        charisma = s.charisma;
        luck = s.luck;
   
    }

    public int GetStatTotal(){
        return maxHealth + def + physical + magic + charisma + luck;
    }

    public int GetStat(StatName stat)
    {
        switch(stat)
        {
            case StatName.HEALTH:
            return maxHealth;
            case StatName.DEFENSE:
            return def;
            case StatName.PHYSICAL:
            return physical;
            case StatName.MAGIC:
            return magic;
            case StatName.CHARISMA:
            return charisma;
            case StatName.LUCK:
            return luck;
            default:
            Debug.LogAssertion("NO SUCH STAT FOUND");
            return 0;
          

        }
    }
}
[System.Serializable]
public class DevNote{
    [TextArea]  public string note;
}

[System.Serializable]
public class BeastData
{
    [Header("DEBUG")]
    public bool ANIMATED_PREFAB_DONE;
    public DevNote devNote;
    [Header("Fluff")]
    [Space()]
    public string beastName;
    [TextArea] public string flavourText;
    [Header("Graphics")]
    [Space()]
    public Vector2 bottomCornerPos = new Vector2(0,-.25f);
    public Vector3 battlePos = new Vector3(0,0,2);
    public GameObject beastGraphicPrefab;
    public Sprite uiPicture;
    public Sprite mainSprite;
    public Sprite outLine;
    public bool facingRight;
    [Header("Attributes")]
    [Space()]
    public int bestiaryID;
    public DeckObject wildDeck;
    public Stats stats;
    public Stats statGrowthPerLevel;
    public Element element;
    public BeastClass beastClass;
    public BeastClass secondaryClass;
    public Passive passive;
    [Range(0,50)] public int catchMod;
}

[CreateAssetMenu(fileName = "New Beast")]
public class BeastScriptableObject : ScriptableObject
{
   public BeastData beastData;
}

