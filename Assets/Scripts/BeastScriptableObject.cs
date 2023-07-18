using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    public int maxHealth;
 

    
    public Stats(Stats s)
    {
        maxHealth = s.maxHealth;
   
    }
}

[System.Serializable]
public class BeastData
{
    [Header("DEBUG")]
    public bool ANIMATED_PREFAB_DONE;
    [Header("Fluff")]
    [Space()]
    public string beastName;
    [TextArea] public string flavourText;
    [Header("Graphics")]
    [Space()]
    public Vector2 bottomCornerPos = new Vector2(0,-1.5f);
    public Vector3 battlePos = new Vector3(0,0,2);
    public GameObject beastGraphicPrefab;
    public Sprite uiPicture;
    public Sprite mainSprite;
    public bool facingRight;
    [Header("Attributes")]
    [Space()]
    public int bestiaryID;
    public Deck wildDeck;
    public Stats stats;
}

[CreateAssetMenu(fileName = "New Beast")]
public class BeastScriptableObject : ScriptableObject
{
   public BeastData beastData;
}

