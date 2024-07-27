using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    public float maxHealth;
    public float maxMana;
    public float manaRegen;
    public float maxDeckCost;
    public float deckCostPerLevel;
 
    public float physical;
    public float magic;
    public float dodge;
    public float luck;


    
    public Stats(Stats s)
    {
        maxHealth = s.maxHealth;
        maxMana = s.maxMana;
        maxDeckCost = s.maxDeckCost;
        deckCostPerLevel = s.deckCostPerLevel;
        manaRegen = s.manaRegen;
        dodge = s.dodge;
        physical = s.physical;
        magic = s.magic;
        luck = s.luck;
   
    }

    public void StackStats(Stats s)
    {
        maxHealth += s.maxHealth;
        maxMana += s.maxMana;
        maxDeckCost += s.maxDeckCost;
        deckCostPerLevel += s.deckCostPerLevel;
        manaRegen += s.manaRegen;
        dodge += s.dodge;
        physical += s.physical;
        magic += s.magic;
        luck +=s.luck;
    }

    public float GetStatTotal(){
        return maxHealth + maxMana + physical + magic + dodge + manaRegen + luck;
    }

    public void ModStat(StatMod statMod)
    {
        switch(statMod.stat)
        {
            case StatName.HEALTH:
            maxHealth += statMod.change;
            break;
            case StatName.MANA:
            maxMana += statMod.change;
            break;
            case StatName.PHYSICAL:
            physical += statMod.change;
            break;
            case StatName.MAGIC:
            magic+= statMod.change;
            break;
            case StatName.DODGE:
            dodge += statMod.change;
            break;
            case StatName.MANAREGEN:
            manaRegen+= statMod.change;
            break;
            case StatName.LUCK:
            luck += statMod.change;
            break;
            default:
            Debug.LogAssertion("NO STAT FOUND");
            break;
        }
    }

    public float GetStat(StatName stat)
    {
        switch(stat)
        {
            case StatName.HEALTH:
            return maxHealth;
            case StatName.MANA:
            return maxMana;
            case StatName.PHYSICAL:
            return physical;
            case StatName.MAGIC:
            return magic;
            case StatName.MANAREGEN:
            return manaRegen;
            case StatName.DODGE:
            return dodge;
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
    public SoundData spawn,die,scream;
    [TextArea] public string flavourText;
    [Header("Graphics")]
    [Space()]
    public float stunBirdHeight = 20;
    public Vector4 playerStatusEffectPos =  new Vector4(0,-4.5f,2,1.5f);
    public Vector4 enemyStatusEffectPos =  new Vector4(0,9,2,1.5f);
    public Vector3 bottomCornerPos = new Vector2(0,-.25f);
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
    public Stats baseStats;
    public Stats  statGrowthPerLevel;
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

