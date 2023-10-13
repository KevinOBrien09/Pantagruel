using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeckEditorCardStack : MonoBehaviour
{
    public TextMeshProUGUI cardName,cardAmount;

    public void Init(Card c){
        cardName.text = c.cardName;
    }

    public void ChangeAmountDisplay(int amount){
        cardAmount.text = "X"+ amount;
    }
  
}
