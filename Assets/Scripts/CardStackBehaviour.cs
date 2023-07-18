using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardStackBehaviour : MonoBehaviour
{
    public TextMeshProUGUI actionName,actionResult;
    public Image actionPic,beastPic;
    public Color32 blue,red;
    public Card savedCard;
    public Beast caster;

    public void Init(Card card,Beast b)
    {
        actionName.text = card.cardName;
        actionPic.sprite = card.picture;
        beastPic.sprite = b.scriptableObject.beastData.uiPicture;
        if(b.scriptableObject.beastData.facingRight){
            beastPic.transform.localScale = new Vector3(-1,1,1);
        }
        else{
            beastPic.transform.localScale =  new Vector3(1,1,1);
        }
        savedCard = card;
        caster = b;
    }
  
}
