using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopCard : MonoBehaviour
{
    public Image mainPicture,classIcon,elementBG,fade;
    public Button frame;
    public TextMeshProUGUI cardName,manaCost,deckCost;
    public Card card;
    public CatalogCardRightClick rightClick;
    public ColorBlock selected,normal;
    public SoundData select,unselect;
    public bool isReward;
    public bool isSelected;
    
    public void Init(Card c,bool r)
    {
        isReward = r;
        card = c;
        mainPicture.sprite = card.picture;
        transform.gameObject.name = card.cardName;
        classIcon.sprite = BeastBank.inst.GetBeastClassSprite(card.beastClass);
        elementBG.color =  BeastBank.inst.GetElementColour(card.element);
        cardName.text = card.cardName;
        manaCost.text = RomanNumerals.ToRoman(card.manaCost);
        deckCost.text = card.deckCost.ToString();
        rightClick.card = c;
       
    }

    public void Disable(){
        frame.interactable = false;
        fade.gameObject.SetActive(true);
    }

    public void Enable(){
        frame.interactable = true;
        fade.gameObject.SetActive(false);
    }

    public void Move(){
        if(isReward)
        {
            if(isSelected){
                frame.colors = normal;;
                isSelected = false;
                AudioManager.inst.GetSoundEffect().Play(unselect);

            }
            else{
                frame.colors = selected;
                isSelected = true;
                           AudioManager.inst.GetSoundEffect().Play(select);
            }
            RewardManager.inst.RefreshCardList(card);
            
        }
    }
}
