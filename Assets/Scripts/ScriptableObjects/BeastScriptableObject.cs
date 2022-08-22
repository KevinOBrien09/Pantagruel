using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    public int maxHealth;
    public int physical;
    public int magic;
    public int toughness;
    public int charisma;
    public int resolve;
    
    public Stats(Stats s)
    {
        maxHealth = s.maxHealth;
        physical = s.physical;
        magic = s.magic;
        toughness = s.toughness;
        charisma = s.charisma;
        resolve = s.resolve;
    }
}

[System.Serializable]
public class BeastData
{
    [Header("DEBUG")]
    public bool ANIMATED_PREFAB_DONE;
    public float renderCamY = 2.3f;
    [Header("Fluff")]
    [Space()]
    public string beastName;
    [TextArea] public string flavourText;
    [Header("Graphics")]
    [Space()]
    public GameObject beastGraphicPrefab;
    public Sprite uiPicture;
    public Sprite mainSprite;
    [Header("Attributes")]
    [Space()]
    public int bestiaryID;
    public Stats stats;
    public Family mainFamily;
    public Family secondaryFamily;
    public SkillResource resource;
    [Header("Skills")]
    [Space()]
    public List<Skill> skills = new List<Skill>();
    public List<HitPip> pips = new List<HitPip>();

    public BeastData(BeastScriptableObject b = null)
    {
        if(b != null)
        {
            BeastData BD = b.beastData;
            ANIMATED_PREFAB_DONE = BD.ANIMATED_PREFAB_DONE;
            beastGraphicPrefab = BD.beastGraphicPrefab;
            beastName = BD.beastName;
            flavourText = BD.flavourText;
            uiPicture = BD.uiPicture;
            mainSprite = BD.mainSprite;
            stats = new Stats(BD.stats);
            resource = new SkillResource(BD.resource);
            skills = new List<Skill>(b.beastData.skills);
            pips = new List<HitPip>(b.beastData.pips);
            renderCamY = BD.renderCamY;
        }
    }
}

[CreateAssetMenu(fileName = "New Beast")]
public class BeastScriptableObject : ScriptableObject
{
   public BeastData beastData;
}

[System.Serializable]
public class HitPip
{
    public int number;
    public HitResult result;
}


