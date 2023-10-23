using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeckEditorCardStack : MonoBehaviour
{
    public TextMeshProUGUI cardName,cardQuanttxt,deckCost;
    int quantity;
    string smallerX = "<size=60%>x</size>";
    Card card;
    public void Init(Card c)
    {
        cardName.text = c.cardName;
        deckCost.text = c.deckCost.ToString();
        card = c;
        Stack();
    }

    public void Stack()
    {
        quantity++;
         
        cardQuanttxt.text = smallerX +  quantity.ToString();
    }

    public void Remove()
    {
        quantity--;
DeckEditor.inst. RefreshFilters(card);
        if(quantity == 0)
        {
            DeckEditor.inst.currentCardStackDict.Remove(card);
            DeckEditor.inst. RefreshFilters(card);
            Destroy(gameObject);
            return;
        }
        cardQuanttxt.text = smallerX +  quantity.ToString();
    }

    public void Click(){
        DeckEditor.inst.MoveFromDeckToCollection(card);
        Remove();
    }
   
}
