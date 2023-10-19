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
    [System.Serializable]
    public class CatalogCardState
    {
        public CardCatalogCard catalogCard;
        public bool dontCare;
        
    }

   
    public ElementTicker unElementalTicker;
    public CardCatagoryTicker commonTicker;
    public Dictionary<Card,CatalogCardState> catalogCardDict = new Dictionary<Card, CatalogCardState>();
    public List<CardCatalogCard> activeCatalogCards = new List<CardCatalogCard>();
    public List<Card> validCards = new List<Card>();
    [SerializeField] CardCatalogCard cardCatalogCardPrefab;
    [SerializeField] DeckEditorBeastButton beastButtonPrefab;
    [SerializeField] DeckEditorCardStack cardStackPrefab;
    [SerializeField] CardCatagoryTicker classTickerPrefab;
    [SerializeField] ElementTicker elementTickerPrefab;
    [SerializeField] GameObject inventoryObject,tickerObject;
    [SerializeField] RectTransform beastButtonHolder,cardCatalogHolder,beastSubmenu,
    subMenuParent,submenuPos1,cardStackHolder,classTickerHolder,elementTickerHolder;
    [SerializeField] GameObject overViewButtons,subMenuButtons;
    [HideInInspector] public DeckEditorBeastButton currentButton;
    Dictionary<Beast,DeckEditorBeastButton> beastButtonDict = new Dictionary<Beast, DeckEditorBeastButton>();
    Dictionary<Card,DeckEditorCardStack> currentCardStackDict = new Dictionary<Card, DeckEditorCardStack>();
    List<GameObject> createdTickers = new List<GameObject>();
    DeckEditorState currentState;
    float beastListShown = -35;
    float beastListHidden =-500;
    bool open;

    public void Start()
    {
        gameObject.SetActive(false);
        overViewButtons.SetActive(false);
        subMenuButtons.SetActive(false);
        tickerObject.SetActive(false);
        open = false;
    }

    public void Toggle()
    {
        if(!open)
        {Open();}
        else
        {Leave();}
    }

    void Open()
    {
        gameObject.SetActive(true);
        inventoryObject.SetActive(false);
        tickerObject.SetActive(true);
        open = true;
        currentState = DeckEditorState.ALLCARDS;
        SpawnBeastList();
        SpawnFullCollection();
        ManageTickers();
    }

    void Leave()
    {
        gameObject.SetActive(false);
        inventoryObject.SetActive(true);
        tickerObject.SetActive(false);
        open = false;
       
        foreach (var item in catalogCardDict)
        {Destroy(item.Value.catalogCard.gameObject);}
        foreach (var item in activeCatalogCards)
        {Destroy(item.gameObject);}
        foreach (var item in createdTickers)
        {Destroy(item.gameObject);}
        foreach (var item in beastButtonDict)
        {Destroy(item.Value.gameObject);}
        foreach (var item in currentCardStackDict)
        {
           Destroy(item.Value.gameObject) ;
        }
        currentCardStackDict.Clear();
        beastButtonDict.Clear();
        catalogCardDict.Clear();
        activeCatalogCards.Clear();
        createdTickers.Clear();
        currentButton = null;
        if(currentState == DeckEditorState.IN_SUB_MENU)
        {beastSubmenu.anchoredPosition = new Vector2(beastListShown,beastSubmenu.anchoredPosition.y);}
         currentState = DeckEditorState.CLOSED;
    }
    
    void SpawnFullCollection()
    {
        foreach (var card in CardCollection.inst.ownedCards)
        {
            if(!catalogCardDict.ContainsKey(card))
            {
                CardCatalogCard ccc = Instantiate(cardCatalogCardPrefab,cardCatalogHolder);
                ccc.Init(card);
                CatalogCardState state = new CatalogCardState();
                state.catalogCard = ccc;
                catalogCardDict.Add(card,state);
            }
            else
            {catalogCardDict[card].catalogCard.Stack();}
        }
        overViewButtons.SetActive(true);
        subMenuButtons.SetActive(false);
        AlphabeticallySortCatalog();
    }

    public void BeastSubMenu(Beast b)
    {
        currentState = DeckEditorState.IN_SUB_MENU;
        currentButton = beastButtonDict[b];
        currentButton.button.interactable = false;
        currentButton.transform.SetParent(subMenuParent.transform);
        currentButton.rt.DOAnchorPos(submenuPos1.anchoredPosition,.2f);
        beastSubmenu.DOAnchorPosX(beastListHidden,.2f);
        SpawnBeastDeck(b);
        overViewButtons.SetActive(false);
        subMenuButtons.SetActive(true);
        List<Card> cardsIDontCareFor = new List<Card>();
        foreach (var item in catalogCardDict)
        {
            BeastData bd = b.scriptableObject.beastData;
            Card c = item.Key;
            bool cardElementIsNone = c. element == Element.NONE;
            bool cardClassIsNone =c.beastClass == BeastClass.COMMON;
            if(cardClassIsNone && cardElementIsNone){
                continue;
            }
            
            if(cardElementIsNone)
            {
                if(c.beastClass != bd.beastClass && c.beastClass != bd.secondaryClass)
                {
                    catalogCardDict[c].dontCare = true;
                    
                    cardsIDontCareFor.Add(c);
                }
            }

            if(cardClassIsNone)
            {
                if(c.element != bd.element || c.element == Element.NONE){
                    catalogCardDict[c].dontCare = true;
                  
                    cardsIDontCareFor.Add(c);
                }
            }

         
            if(c.element != bd.element)
            {
                if(c.beastClass != bd.beastClass)
                {
                    if(c.beastClass != bd.secondaryClass)
                    {
                        catalogCardDict[c].dontCare = true;
                        
                        cardsIDontCareFor.Add(c);
                    }
                }
            }
         
        }

        foreach (var item in  catalogCardDict)
        {
            if(item.Value.dontCare){
                item.Value.catalogCard.gameObject.SetActive(false);
            }
              
        }
        ManageTickers();

    }

    public void ManageTickers()
    {
        foreach (var item in createdTickers)
        {Destroy(item);}
        createdTickers.Clear();

        var bc = Enum.GetValues(typeof(BeastClass));
        List<BeastClass> classBin = new List<BeastClass>();

        var el = Enum.GetValues(typeof(BeastClass));
        List<Element> elementBin = new List<Element>();

        foreach (BeastClass beastClass in bc)
        {
            bool  x = false;
            foreach (var card in catalogCardDict)
            {
                if(card.Key.beastClass == beastClass)
                {
                    if(!card.Value.dontCare)
                    {
                        x = true;
                        break;
                    }
                }
            }
            if(!x)
            {classBin.Add(beastClass);}
        }
        
        foreach (BeastClass item in bc)
        {
            if(!classBin.Contains(item))
            {
                if(item != BeastClass.COMMON)
                {
                    CardCatagoryTicker t = Instantiate(classTickerPrefab,classTickerHolder);
                    t.Init(item);
                    createdTickers.Add(t.gameObject);
                }
            
            }   
        }


        foreach (Element element in el)
        {
            bool  x = false;
            foreach (var card in catalogCardDict)
            {
                if(card.Key.element == element)
                {
                    if(!card.Value.dontCare)
                    {
                        x = true;
                        break;
                    }
                }
            }
            if(!x)
            {elementBin.Add(element);}
        }
        
        foreach (Element element in el)
        {
            if(!elementBin.Contains(element))
            {
                if(element != Element.NONE)
                {
                    ElementTicker t = Instantiate(elementTickerPrefab,elementTickerHolder);
                    t.Init(element);
                    createdTickers.Add(t.gameObject);
                }
            
            }   
        }
    }

    public void LeaveSubMenu()
    {
        currentButton.transform.SetParent(beastButtonHolder);
        beastSubmenu.DOAnchorPosX(beastListShown,.2f);
        WipeBeastDeck();
        SortBeastButtonsByPartyOrder();
        overViewButtons.SetActive(true);
        subMenuButtons.SetActive(false);
        currentButton.button.interactable = true;
 currentButton = null;
        foreach (var item in  catalogCardDict)
        {
            item.Value.catalogCard.gameObject.SetActive(true);
            item.Value.dontCare = false;
        }
        ManageTickers();
    }

   
    void WipeBeastDeck()
    {
        foreach (var item in currentCardStackDict)
        {
           Destroy(item.Value.gameObject) ;
        }
        currentCardStackDict.Clear();
    }

    void SpawnBeastDeck(Beast b)
    {
        foreach (var item in b.deck.cards)
        {
            if(!currentCardStackDict.ContainsKey(item))
            {
                DeckEditorCardStack stack = Instantiate(cardStackPrefab,cardStackHolder);
                stack.Init(item);
                currentCardStackDict.Add(item,stack);
            }
            else
            {
                currentCardStackDict[item].Stack();
            }
        }
    }

    void SpawnBeastList()
    {
        foreach (var item in PlayerParty.inst.party)
        {
            DeckEditorBeastButton b = Instantiate(beastButtonPrefab,beastButtonHolder);
            beastButtonDict.Add(item,b);
            b.Init(item);
            
        }
    }

    void SortBeastButtonsByPartyOrder()
    {
        int i = 0;
        foreach (var item in PlayerParty.inst.party)
        {
            beastButtonDict[item].transform.SetSiblingIndex(i);
            i++;
        }
    }

    void AlphabeticallySortCatalog()
    {
        List<CardCatalogCard> ca = new List<CardCatalogCard>();
        foreach (var card in catalogCardDict)
        {ca.Add(card.Value.catalogCard);}

        ca = ca.OrderBy(x => (int)x.card.beastClass).ToList();
        List<CardCatalogCard> bin = new List<CardCatalogCard>();
        int d = 0;
        foreach (var card in ca)
        {
            if(card.card.beastClass == BeastClass.COMMON)
            {
                card.transform.SetSiblingIndex(d); 
                bin.Add(card);
                d++;
            }
        }
        foreach (var card in bin)
        {ca.Remove(card);}
        ca = ca.OrderBy(x => x.card.beastClass.ToString()).ToList();
        foreach (var card in ca)
        { 
            card.transform.SetSiblingIndex(d); 
            d++;
        }
    }
}