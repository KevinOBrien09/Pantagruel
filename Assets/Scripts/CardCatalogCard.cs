using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardCatalogCard : MonoBehaviour
{
    public Image mainPicture,classIcon,elementBG;
    public TextMeshProUGUI cardName,manaCost,deckCost,quantityText;
    public Card card;
    public CatalogCardRightClick rightClick;
    int quantity;
    string smallerX = "<size=60%>x</size>";

    public void Init(Card c)
    {
        card = c;
        mainPicture.sprite = card.picture;
        transform.gameObject.name = card.cardName;
        classIcon.sprite = BeastBank.inst.GetBeastClassSprite(card.beastClass);
        elementBG.color =  BeastBank.inst.GetElementColour(card.element);
        cardName.text = card.cardName;
        manaCost.text = RomanNumerals.ToRoman(card.manaCost);
        deckCost.text = card.deckCost.ToString();
        rightClick.catalogCard = this;
        Stack();
    }

    public void Stack()
    {   
        quantity++;
          
        quantityText.text = smallerX + quantity.ToString();
    }

    public void Remove()
    {   
        quantity--;
        if(quantity == 0)
        {
            DeckEditor.inst.activeCatalogCards.Remove(this);
            DeckEditor.inst.catalogCardDict.Remove(card);
          
            DeckEditor.inst.ChangeCountText();
            DeckEditor.inst. RefreshFilters(card);
            Destroy(gameObject);
            return;
        }
        quantityText.text = smallerX + quantity.ToString();
    }

    public void Click()
    {
        if(DeckEditor.inst.currentState == DeckEditorState.IN_SUB_MENU)
        {
           if( DeckEditor.inst.MoveFromCollectionToDeck(card))
           {Remove();}
        }
    }
}