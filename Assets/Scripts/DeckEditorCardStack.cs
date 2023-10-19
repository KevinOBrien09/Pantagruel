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
    public void Init(Card c)
    {
        cardName.text = c.cardName;
        deckCost.text = c.deckCost.ToString();
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
        cardQuanttxt.text = smallerX +  quantity.ToString();
    }
   
}
