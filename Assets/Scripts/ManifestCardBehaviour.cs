using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ManifestCardBehaviour : MonoBehaviour
{
    public Image mainPicture,fade,paper;
    public Button frame;
    public TextMeshProUGUI cardName,manaCost;
    public Card card;
    public CatalogCardRightClick rightClick;
    public ColorBlock selected,normal;
    public SoundData select,unselect;
    public bool isSelected;
    
    public void Init(Card c,CardManager.CardState state)
    {
       if(state == CardManager.CardState.NORMAL){
        mainPicture.material = null;
        paper.color = Color.white;
       }
        card = c;
        mainPicture.sprite = card.picture;
        transform.gameObject.name = card.cardName;
       
        cardName.text = card.cardName;
        manaCost.text = RomanNumerals.ToRoman(card.manaCost);
     
        rightClick.card = c;
    }

    public void Click(){
        if(CardViewer.inst.gameObject.activeSelf){
            return;
        }
        else{
            CardManifester.inst.SelectedCard(card);
        }
    }
}