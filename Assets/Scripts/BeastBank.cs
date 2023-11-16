using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastBank : Singleton<BeastBank>
{
    public List<BeastScriptableObject> bank = new List<BeastScriptableObject>();
    public List<Card> cardBank = new List<Card>();
    public Dictionary<int,BeastScriptableObject> beastDict = new Dictionary<int, BeastScriptableObject>();
    public Dictionary<string,Card> cardDict = new Dictionary<string,Card>();
    public Dictionary<BeastClass,Sprite> beastClassSpriteDict =  new Dictionary<BeastClass, Sprite>();
    public Dictionary<Element,Color32> elementColourDict =  new Dictionary<Element,Color32>();
    public List<BeastClassPic> beastClassPics = new List<BeastClassPic>();
    public List<ElementColours> elementColours = new List<ElementColours>();
    public Sprite missing;

    [System.Serializable]
    public class BeastClassPic
    {
        public BeastClass beastClass;
        public Sprite sprite;
    }

    [System.Serializable]
    public class ElementColours
    {
        public Element element;
        public Color32 colour;
    }

    protected override void Awake()
    {
        base.Awake();
        foreach (var item in bank)
        {beastDict.Add(item.beastData.bestiaryID,item);}

        foreach (var item in cardBank)
        {cardDict.Add(item.Id,item);}

        foreach (var item in beastClassPics)
        {beastClassSpriteDict.Add(item.beastClass,item.sprite);}

        foreach (var item in elementColours)
        {elementColourDict.Add(item.element,item.colour);}
    }

    public Sprite GetBeastClassSprite(BeastClass bClass){
        if(beastClassSpriteDict.ContainsKey(bClass))
        {
            return beastClassSpriteDict[bClass];
        }
        else{
            return missing;
        }

    }

     public Color32 GetElementColour(Element element){
        if(elementColourDict.ContainsKey(element))
        {
            return elementColourDict[element];
        }
        else{
            Debug.LogAssertion("No colour found");
            return Color.white;
        }

    }
}
