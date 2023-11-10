using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class CardStackBehaviour : MonoBehaviour
{
    public TextMeshProUGUI cardName,actionResult;
    public Image cardPic,bg;
    public Color32 blue,red;
    public Card savedCard;
    public Beast caster;
    public GameObject promiseParent;

    public void Init(Card card,Beast b,bool isPlayer)
    {
        if(isPlayer){
            bg.color = blue;
        }
        else{
            bg.color = red;
        }
        cardName.text = card.cardName;
        cardPic.sprite = card.picture;

        savedCard = card;
        caster = b;

       if (card.effects .OfType<Promise>().Any()){
        promiseParent.gameObject.SetActive(true);
       }
       else{
        promiseParent.gameObject.SetActive(false);
       }
    }
  
}
