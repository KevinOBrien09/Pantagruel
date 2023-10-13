using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardCatalogCard : MonoBehaviour
{
    public Image pic;
    public TextMeshProUGUI cardName,cardCost;

    public void Init(Card card)
    {
        cardName.text = card.cardName;
        pic.sprite = card.picture;
        cardCost.text = RomanNumerals.ToRoman(card.manaCost);
    }

}