using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CardCombo
{
   
    public List<Card> requiredCards;
    public int fullManaCost()
    {
        int i = 0;
        foreach (var item in requiredCards)
        {i += item.manaCost;}
        return i;
    }
}

 