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
 
    bool unclickable;
    public Image frame;
    public Sprite parasiteFrame;
    public void Init(Card c)
    {
        cardName.text = c.cardName;
        deckCost.text = c.deckCost.ToString();
        card = c;
        Stack();
        if(c.Id == ParasiteEffect.parasiteCard){
            unclickable = true;
            frame.sprite = parasiteFrame;
            cardName.text = "<b><color=#FF00BD>" + cardName.text;
        }
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
        if(unclickable){
            return; 
        }
        DeckEditor.inst.MoveFromDeckToCollection(card);
        Remove();
    }
   
}
