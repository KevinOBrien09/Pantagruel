using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardCatalogCard : MonoBehaviour
{
    public Image mainPicture,classIcon,elementBG;
    public TextMeshProUGUI cardName,manaCost,deckCost,quantityText;
    public Card card;

    int quantity;
    string smallerX = "<size=60%>x</size>";
    public void Init(Card c)
    {
        card = c;
        mainPicture.sprite = card.picture;
        classIcon.sprite = BeastBank.inst.GetBeastClassSprite(card.beastClass);
        elementBG.color =  BeastBank.inst.GetElementColour(card.element);
        cardName.text = card.cardName;
        manaCost.text = RomanNumerals.ToRoman(card.manaCost);
        deckCost.text = card.deckCost.ToString();
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
        quantityText.text = smallerX + quantity.ToString();


    }
}