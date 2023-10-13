using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using DG.Tweening;

public enum DeckEditorState{ALLCARDS,CLOSED,IN_SUB_MENU}

public class DeckEditor : Singleton<DeckEditor>
{
    public CardCatagoryTicker catagoryTicker;
    public Transform holder;
    public GameObject tickerWindow;
    public bool open;
    Dictionary<BeastClass,CardCatagoryTicker> activeTickers = new Dictionary<BeastClass,CardCatagoryTicker>();
    public DeckEditorBeastButton beastButPrefab;
    public RectTransform beastButtonHolder,editDeckSubMenu;
    public List<DeckEditorBeastButton> activeButtons = new List<DeckEditorBeastButton>();
    public Transform child1,beastBrowserParent,beastDeckList;
    public RectTransform pos1,pos2;
    public VerticalLayoutGroup beastLayoutGroup;
    public Image subMenuBeastBG;
    public DeckEditorCardStack cardStackPrefab;
    Dictionary<Card,StackQuantPair> currentDeck = new Dictionary<Card,StackQuantPair>();
    public Beast beastWhosDeckIsBeingEdited;
    public DeckEditorState deckEditorState;
    public GameObject subMenuButtons;

    public class StackQuantPair
    {
        public  int quant;
        public   DeckEditorCardStack cardStack;
    }

    void Start()
    {
        foreach (Transform item in holder)
        {Destroy(item.gameObject);}
        deckEditorState = DeckEditorState.CLOSED;
        var beastClasses = Enum.GetValues(typeof(BeastClass));
        gameObject.SetActive(false);
        tickerWindow.SetActive(false);
        open = true;
    }

    public void Open()
    {
        open = !open;
        if(open)
        {
            deckEditorState = DeckEditorState.CLOSED;
            beastWhosDeckIsBeingEdited = null;
            gameObject.SetActive(false);
            tickerWindow.SetActive(false);
        }
        else
        {
            CreateTickers(ClassesOfOwnedCards(),true);
            deckEditorState = DeckEditorState.ALLCARDS;
            CreateBeastButtons();
            CardCatalog.inst.FilterMassEdit(new List<Element>(),ClassesOfOwnedCards());   
            gameObject.SetActive(true);
            tickerWindow.SetActive(true);
        }
    }

    public void CreateBeastButtons()
    {
        foreach (var item in activeButtons)
        {
            Destroy(item.gameObject);
        }
        activeButtons.Clear();
        foreach (var item in PlayerParty.inst.party)
        {
            DeckEditorBeastButton deckEditorBeastButton = Instantiate(beastButPrefab,beastButtonHolder);
            deckEditorBeastButton.Init(item);
            activeButtons.Add(deckEditorBeastButton);
        }
    }
    
    public void CreateTickers(List<BeastClass> filters,bool clickable)
    {
        foreach (var item in activeTickers)
        {Destroy(item.Value.gameObject);}
        activeTickers.Clear();
        foreach (BeastClass item in filters )
        {
            CardCatagoryTicker ticker = Instantiate(catagoryTicker,holder);
            ticker.Init(item,clickable);
            activeTickers.Add(item,ticker);
        }
    }

    public bool PlayerOwnsCardOfThisClass(BeastClass beastClass)
    {
        foreach (var item in CardCollection.inst.ownedCards)
        {
            if(item.restrictions.classRestriction == beastClass)
            {return true;}
        }
        return false;
    }

    public List<BeastClass> ClassesOfOwnedCards()
    {
        List<BeastClass> c = new List<BeastClass>();
        foreach (var item in CardCollection.inst.ownedCards)
        {
            if(!c.Contains(item.restrictions.classRestriction))
            {c.Add(item.restrictions.classRestriction);}
        }
        return c;
    }
    
  

    public void SelectBeast(Beast b,DeckEditorBeastButton button)
    {
        beastWhosDeckIsBeingEdited = b;
        Debug.Log(b.scriptableObject.beastData.beastName);
        button.button.interactable = false;
        deckEditorState = DeckEditorState.IN_SUB_MENU;
        //beastLayoutGroup.enabled = false;
        editDeckSubMenu.DOAnchorPosX(-600,0);
        editDeckSubMenu.gameObject.SetActive(true);button.transform.SetParent(child1);
        button.rt.DOAnchorPos(pos1.anchoredPosition,.2f);beastButtonHolder.DOAnchorPos(pos2.anchoredPosition,.25f).OnComplete(
            
            ()=> 
            {
                beastBrowserParent.gameObject.SetActive(false);
                subMenuButtons.SetActive(true);
            }
            
            );
        editDeckSubMenu.DOAnchorPosX(-100,.25f);
        subMenuBeastBG.sprite = b.scriptableObject.beastData.mainSprite;
        if(!b.scriptableObject.beastData.facingRight)
        {subMenuBeastBG.transform.rotation = Quaternion.Euler(0,-180,0);}
       
        List<BeastClass> beastClasses = GetBeastClasses(b);
        
        CreateTickers(beastClasses,true);
        foreach (var item in activeTickers)
        {
            if(!PlayerOwnsCardOfThisClass(item.Key))
            {
                item.Value.GreyOut();
            }
        }
        // CardCatalog.inst.Wipe();
        // CardCatalog.inst.CreateCards(new List<Element>(),beastClasses);
        CardCatalog.inst.FilterMassEdit(new List<Element>(),beastClasses);
        SpawnCardList(b);
    }

    public List<BeastClass> GetBeastClasses(Beast b)
    {
        List<BeastClass> beastClasses = new List<BeastClass>();
        beastClasses.Add(BeastClass.COMMON);
        beastClasses.Add( b.scriptableObject.beastData.beastClass);
        if( b.scriptableObject.beastData.secondaryClass != BeastClass.COMMON)
        { beastClasses.Add( b.scriptableObject.beastData.secondaryClass);}

        return beastClasses;
    }

    void SpawnCardList(Beast b)
    {
        foreach (var card in b.deck.cards)
        {
            if(!currentDeck.ContainsKey(card))
            {
                DeckEditorCardStack c = Instantiate(cardStackPrefab,beastDeckList);
                c.Init(card);
                StackQuantPair q = new StackQuantPair();
                q.cardStack = c;
                q.quant = 0;
                currentDeck.Add(card,q);
                q.quant++;
                c.ChangeAmountDisplay(q.quant);
            }
            else
            {
                currentDeck[card].quant++;
                currentDeck[card].cardStack .ChangeAmountDisplay( currentDeck[card].quant);
            }
           
        }

    }

}