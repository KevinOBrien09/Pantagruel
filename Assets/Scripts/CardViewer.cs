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
    public TextMeshProUGUI cardName,manaCost,deckCost,description,flavourText,summonTopDesc,summonBottomDesc,summonFlavour;
    public Image classIcon,elementBG,mainPicture;
    public ToolTip[] toolTips;
    public GameObject[] descHolders;
    public List<string> toolTipText = new List<string>();
    public Transform catalogHolder;
    public Button right,left,changeDescStateBut;
    public CardDescState descState;
    public bool manuallyLoadCards;
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
        SetDesc(card);
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
        if(!manuallyLoadCards){
       RefreshCardList();
        }
 
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

    public void SetDesc(Card card)
    {
        foreach (var item in descHolders)
        {item.SetActive(false);}
        (SummonEffect, bool) t =   checkForSummon(card.effects);
        bool b = t.Item2;
        SummonEffect se = t.Item1;

        if(b)
        {
            descHolders[1].SetActive(true);
            summonTopDesc.text = card.desc;
            summonBottomDesc.text =  se.pet.petEffect.desc;
            summonFlavour.text = card.flavourText;

        }
        else
        {
            descHolders[0].SetActive(true);
            description.text = CardDescParser.GetBeastValues(card,PlayerParty.inst.activeBeast);
            descState = CardDescState.VALUE;
            flavourText.text = card.flavourText;
        }

        (SummonEffect, bool) checkForSummon(List<Effect> cardEffects)
        {
            foreach(Effect e in cardEffects)
            {if(e is SummonEffect) return ((SummonEffect) e, true);}
            return (null, false);
        }
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

    public void ManuallyLoadCards(List<Card> newCards){
        cards.Clear();
        cards = new List<Card>(newCards);
    }

    
    public void Right()
    {
        index++;
        LoadCard(cards[index]);
    }

    public void Close()
    {
        if(gameObject.activeSelf)
        {AudioManager.inst.GetSoundEffect().Play(SystemSFX.inst.closeWindow); }

        if(BattleManager.inst.inBattle)
        {
            if(!CardManifester.inst.isManifesting){
CardManager.inst.ActivateHand();
EndTurnButton.inst.Reactivate();
            cards.Clear();
            manuallyLoadCards = false;
            }
            
        }
        
        gameObject.SetActive(false);
    }

}