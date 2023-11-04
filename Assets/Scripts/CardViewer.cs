using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;

public class CardViewer : Singleton<CardViewer>
{
    public enum CardDescState{PERCENTAGE,VALUE}
    public TextMeshProUGUI cardName,manaCost,deckCost,description,flavourText;
    public Image classIcon,elementBG,mainPicture;
    public ToolTip[] toolTips;
    public List<string> toolTipText = new List<string>();
    public Transform catalogHolder;
    public Button right,left,changeDescStateBut;
    public CardDescState descState;
    Card currentCard;
    int index;
    List<Card> cards = new List<Card>();
    void Start(){
        gameObject.SetActive(false);
    }

    public void Open(Card card)
    {
        
        gameObject.SetActive(true);
        LoadCard(card);
    }

    void LoadCard(Card card)
    {
        AudioManager.inst.GetSoundEffect().Play(DeckEditor.inst.openCardViewer);
        currentCard = card;
        cardName.text = card.cardName;
        manaCost.text = RomanNumerals.ToRoman(card.manaCost);
        deckCost.text = card.deckCost.ToString();
        RefreshDescStateButton();
        description.text = CardDescParser.GetBeastValues(card,PlayerParty.inst.activeBeast);
        descState = CardDescState.VALUE;
        //card.desc;
        flavourText.text = card.flavourText;
        classIcon.sprite = BeastBank.inst.GetBeastClassSprite(card.beastClass);
        elementBG.color = BeastBank.inst.GetElementColour(card.element);
        mainPicture.sprite = card.picture;
        toolTips[0].Init(toolTipText[0] + card.manaCost.ToString() );
        string e = card.element.ToString();
        string bc = card.beastClass.ToString();
        if(card.element == Element.NONE){
            e = "unelemental";
        }
   
        toolTips[1].Init( e+ "/" + bc);
        toolTips[2].Init(toolTipText[2] + card.deckCost.ToString() );

        RefreshCardList();
        index = cards.IndexOf(card);
   
        if(index == 0)
        {left.gameObject.SetActive(false);}
        else
        {left.gameObject.SetActive(true);}
        if(index==cards.Count-1)
        {right.gameObject.SetActive(false);}
        else
        {right.gameObject.SetActive(true);}
    }

    public void ChangeDescState(){
            if(descState == CardDescState.VALUE){
                description.text = CardDescParser.GetPercentValues(currentCard);
                descState = CardDescState.PERCENTAGE;
            }
            else{
                description.text = CardDescParser.GetBeastValues(currentCard,PlayerParty.inst.activeBeast);
                descState = CardDescState.VALUE;
            }
    }

    void RefreshDescStateButton(){
        if(CardDescParser.cardIsSwitchable(currentCard.desc)){
            changeDescStateBut.gameObject.SetActive(true);
        }
        else{
            changeDescStateBut.gameObject.SetActive(false);
        }
    }

    public void Left()
    {
        
        index--;
        LoadCard(cards[index]);
    }

    void RefreshCardList()
    {
        cards.Clear();
        foreach (Transform item in catalogHolder.transform)
        {
            if(item.gameObject.activeSelf)
            {cards.Add(item.gameObject.GetComponent<CardCatalogCard>().card);}
        }
    }

    
    public void Right()
    {
        index++;
        LoadCard(cards[index]);
    }

    public void Close(){
        gameObject.SetActive(false);
    }

}