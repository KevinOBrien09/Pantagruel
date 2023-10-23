using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using DG.Tweening;
using TMPro;

public enum DeckEditorState{ALLCARDS,CLOSED,IN_SUB_MENU}

public class DeckEditor : Singleton<DeckEditor>
{
    [System.Serializable]
    public class CatalogCardState
    {
        public CardCatalogCard catalogCard;
        public bool dontCare;
    }
    
    public DeckEditorState currentState;
    public ElementTicker unElementalTicker;
    public CardCatagoryTicker commonTicker;
    public Dictionary<Card,CatalogCardState> catalogCardDict = new Dictionary<Card, CatalogCardState>();
    public Dictionary<Card,DeckEditorCardStack> currentCardStackDict = new Dictionary<Card, DeckEditorCardStack>();
    public List<CardCatalogCard> activeCatalogCards = new List<CardCatalogCard>();
    public string searchString;
    public SoundData moveCard,openCardViewer,openBeastMenu,leaveBeastMenu,tickerToggle;
    [SerializeField] CardCatalogCard cardCatalogCardPrefab;
    [SerializeField] DeckEditorBeastButton beastButtonPrefab;
    [SerializeField] DeckEditorCardStack cardStackPrefab;
    [SerializeField] CardCatagoryTicker classTickerPrefab;
    [SerializeField] ElementTicker elementTickerPrefab;
    [SerializeField] GameObject inventoryObject,tickerObject;
    [SerializeField] Image bgBeastImage;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI cardCount;
    [SerializeField] RectTransform beastButtonHolder,cardCatalogHolder,beastSubmenu,
    subMenuParent,submenuPos1,cardStackHolder,classTickerHolder,elementTickerHolder,subMenuButtonHolder;
    [SerializeField] GameObject overViewButtons,subMenuButtons;
    [HideInInspector] public DeckEditorBeastButton currentButton;
    Dictionary<Beast,DeckEditorBeastButton> beastButtonDict = new Dictionary<Beast, DeckEditorBeastButton>();
    List<GameObject> createdTickers = new List<GameObject>();
    public List<BeastClass> shownClasses = new List<BeastClass>();
    public List<Element> shownElements = new List<Element>();
    float beastListShown = -35;
    float beastListHidden =-500;
    bool open;

    //new ticker is not created when moving from deck to catalog and a previous one did not exist.

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
        OverworldMovement.canMove = false;
        bgBeastImage.DOFade(0,0);
        gameObject.SetActive(true);
        inventoryObject.SetActive(false);
        tickerObject.SetActive(true);
        open = true;
        currentState = DeckEditorState.ALLCARDS;
        SpawnBeastList();
        SpawnFullCollection();
        ChangeCountText();
        ManageTickers();
    }

   public void ChangeCountText(){
        int i = 0;
        foreach (var item in catalogCardDict)
        {
            if(item.Value.catalogCard.gameObject.activeSelf){
                i++;
            }
            
        }
            cardCount.text =  i.ToString() + "/" + catalogCardDict.Count + " cards shown";
    }

    void Leave()
    {
        OverworldMovement.canMove = true;
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
        {Destroy(item.Value.gameObject);}
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


    public bool MoveFromCollectionToDeck(Card c)
    {
        Deck d = currentButton.beast.deck;
        if( d.AddCardToDeck(c)){
            CardCollection.inst.RemoveCard(c);
               AudioManager.inst.GetSoundEffect().Play(moveCard);
            if(currentCardStackDict.ContainsKey(c))
            {currentCardStackDict[c].Stack();}
            else
            {
                DeckEditorCardStack stack = Instantiate(cardStackPrefab,cardStackHolder);
                stack.Init(c);
                currentCardStackDict.Add(c,stack);
            }
            currentButton.UpdateDeckCostMeter();
             
            return true;
        }
        else
        {
            Debug.Log("error sound here");
            return false;
        }
       
    }

    public void Search()
    {
        searchString = inputField.text.Replace('_',' ');;
        inputField.text = inputField.text.Replace(' ','_');
        ToggleCards();
    }

    public void MoveFromDeckToCollection(Card c)
    {
        Deck d = currentButton.beast.deck;
        CardCollection.inst.AddCard(c);
        d.RemoveCardFromDeck(c);
        AudioManager.inst.GetSoundEffect().Play(moveCard);
        if(catalogCardDict.ContainsKey(c))
        {catalogCardDict[c].catalogCard.Stack();}
        else
        {
            CardCatalogCard ccc = Instantiate(cardCatalogCardPrefab,cardCatalogHolder);
            ccc.Init(c);
            CatalogCardState state = new CatalogCardState();
            state.catalogCard = ccc;
            catalogCardDict.Add(c,state);
            if(!shownClasses.Contains(c.beastClass))
            {shownClasses.Add(c.beastClass);}
            
            if(!shownElements.Contains(c.element))
            {shownElements.Add(c.element);}
           
            ToggleCards();
            AlphabeticallySortCatalog();
        }
      
        currentButton.UpdateDeckCostMeter();
        ChangeCountText();
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
        if( b.scriptableObject.beastData.facingRight)
        {bgBeastImage.transform.rotation = Quaternion.Euler(0,0,0);}
        else{bgBeastImage.transform.rotation = Quaternion.Euler(0,-180,0);}
        bgBeastImage.DOFade(.2f,.5f);
        AudioManager.inst.GetSoundEffect().Play(openBeastMenu);
        bgBeastImage.sprite = b.scriptableObject.beastData.mainSprite;
        currentButton.button.interactable = false;
        currentButton.transform.SetParent(subMenuButtonHolder.transform);
        currentButton.rt.DOAnchorPos(submenuPos1.anchoredPosition,.2f);
        beastSubmenu.DOAnchorPosX(beastListHidden,.2f);
        SpawnBeastDeck(b);
        overViewButtons.SetActive(false);
        subMenuButtons.SetActive(true);
    
        foreach (var item in catalogCardDict)
        {
            BeastData bd = b.scriptableObject.beastData;
            Card c = item.Key;
            bool cardElementIsNone = c.element == Element.NONE;
            bool cardClassIsNone =c.beastClass == BeastClass.COMMON;
            if(cardClassIsNone && cardElementIsNone)
            { continue; }
            
            if(cardElementIsNone)
            {
                if(c.beastClass != bd.beastClass && c.beastClass != bd.secondaryClass)
                {catalogCardDict[c].dontCare = true;}
            }

            if(cardClassIsNone)
            {
                if(c.element != bd.element || c.element == Element.NONE)
                {catalogCardDict[c].dontCare = true;}
            }
            
            if(c.element != bd.element)
            {
                if(c.beastClass != bd.beastClass)
                {
                    if(c.beastClass != bd.secondaryClass)
                    {catalogCardDict[c].dontCare = true;}
                }
            }
        }

        if(IsThereStillACardOfThisElementInTheCurrentCollection(Element.NONE)) 
        {unElementalTicker.UnGreyOut();}
        else
        {
            unElementalTicker.GreyOut();
        }
        if(IsThereStillACardOfThisClassInTheCurrentCollection(BeastClass.COMMON)) 
        {commonTicker.UnGreyOut();}
        else
        {
            commonTicker.GreyOut();
        }

        ManageTickers();
        ToggleCards();
    }

    public void RefreshFilters(Card c)
    {
        var el = Enum.GetValues(typeof(Element));
        var bc = Enum.GetValues(typeof(BeastClass));
        List<Card> relevantCards = new List<Card>();
        foreach (Element element in el)
        {
            foreach (var card in catalogCardDict)
            {
                if(!card.Value.dontCare)
                {
                    if(!relevantCards.Contains(card.Key))
                    {relevantCards.Add(card.Key);}
                }
            }
        }
        List<Element> elementsOfOwnedCards = new List<Element>();
        List<BeastClass> classesOfOwnedCards = new List<BeastClass>();
        Dictionary<Element,ElementTicker> elementTickers = new Dictionary<Element, ElementTicker>();
        Dictionary<BeastClass,CardCatagoryTicker> classTickers = new Dictionary<BeastClass,CardCatagoryTicker>();
        elementTickers.Add(Element.NONE,unElementalTicker);
        classTickers.Add(BeastClass.COMMON,commonTicker);

    	foreach (var item in createdTickers)
        {
            CardCatagoryTicker cardCatagoryTicker = null;
            ElementTicker elementTicker = null;
            if(item.gameObject.TryGetComponent<CardCatagoryTicker>(out cardCatagoryTicker))
            {classTickers.Add(cardCatagoryTicker.beastClass,cardCatagoryTicker);}

            if(item.gameObject.TryGetComponent<ElementTicker>(out elementTicker))
            {elementTickers.Add(elementTicker.element,elementTicker);}
            
        }

        foreach (var item in relevantCards)
        {
            if(!elementsOfOwnedCards.Contains(item.element))
            {elementsOfOwnedCards.Add(item.element);}
            
            if(!classesOfOwnedCards.Contains(item.beastClass))
            {classesOfOwnedCards.Add(item.beastClass);}
        }

    

        foreach (var item in elementTickers)
        {
            item.Value.GreyOut();

            if(elementsOfOwnedCards.Contains(item.Value.element))
            {
                item.Value.UnGreyOut();
         
            }
        }

        foreach (var item in classTickers)
        {
            item.Value.GreyOut();

            if(classesOfOwnedCards.Contains(item.Value.beastClass))
            {
                item.Value.UnGreyOut(); 
            }
        }

   
        foreach (Element item in el)
        {
            if(elementsOfOwnedCards.Contains(item))
            {
                if(!elementTickers.ContainsKey(item))
                {
                    ElementTicker t = Instantiate(elementTickerPrefab,elementTickerHolder);
                    t.Init(item);
                    shownElements.Add(item);
                    createdTickers.Add(t.gameObject);
                }
            }
        }

        foreach (BeastClass item in bc)
        {
            if(classesOfOwnedCards.Contains(item))
            {
                if(!classTickers.ContainsKey(item))
                {
                    CardCatagoryTicker t = Instantiate(classTickerPrefab,classTickerHolder);
                    t.Init(item);
                    shownClasses.Add(item);
                    createdTickers.Add(t.gameObject);
                }
            }
        }
    }

    public void ManageTickers()
    {
        foreach (var item in createdTickers)
        {Destroy(item);}
        createdTickers.Clear();
        shownClasses.Clear();
        shownElements.Clear();

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
                    shownClasses.Add(item);
                    t.Init(item);
                    createdTickers.Add(t.gameObject);
                }
                else
                {
                    shownClasses.Add(BeastClass.COMMON);
                    commonTicker.toggle.enabled = true;
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
                    shownElements.Add(element);
                    createdTickers.Add(t.gameObject);
                }
                else
                {
                    shownElements.Add(Element.NONE);
                    unElementalTicker.toggle.enabled = true;
                }
            }   
        }
    }
    
    public bool FilterClass(BeastClass bc)
    {
        if(shownClasses.Contains(bc))
        {
            shownClasses.Remove(bc);
            ToggleCards();

            return false;
        }
        else
        {
            shownClasses.Add(bc);
            ToggleCards();

            return true;
        }
    }

    public bool FilterElement(Element element)
    {
        if(shownElements.Contains(element))
        {
            shownElements.Remove(element);
            ToggleCards();
            return false;
        }
        else
        {
            shownElements.Add(element);
            ToggleCards();
            return true;
        }
    }

   

    public void ToggleCards()
    {
        foreach (var item in catalogCardDict)
        {
            
            if(!item.Value.dontCare)
            {
                bool elementIsFiltered = !shownElements.Contains(item.Key.element);
                bool classIsFiltered = !shownClasses.Contains(item.Key.beastClass);
                if(elementIsFiltered && classIsFiltered)
                {item.Value.catalogCard.gameObject.SetActive(false);}
                else if(elementIsFiltered && !classIsFiltered)
                {item.Value.catalogCard.gameObject.SetActive(false);}
                else if(!elementIsFiltered && classIsFiltered)
                {item.Value.catalogCard.gameObject.SetActive(false);}
                else
                {item.Value.catalogCard.gameObject.SetActive(true);}


                if(searchString != string.Empty)
            {
                if(item.Key.cardName.ToUpper().Contains(searchString.ToUpper()))
                {
                    item.Value.catalogCard.gameObject.SetActive(true);
                }
                else
                {
                    item.Value.catalogCard.gameObject.SetActive(false);
                }
            }
            }
            else
            {item.Value.catalogCard.gameObject.SetActive(false);}
        }

        ChangeCountText();
    }

    public void LeaveSubMenu()
    {
        currentState = DeckEditorState.ALLCARDS;
        currentButton.transform.SetParent(beastButtonHolder);
        beastSubmenu.DOAnchorPosX(beastListShown,.2f);
        inputField.text = "";
        searchString = "";
        AudioManager.inst.GetSoundEffect().Play(leaveBeastMenu);
    

        bgBeastImage.DOFade(0,.2f);
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
            if(IsThereStillACardOfThisElementInTheCurrentCollection(Element.NONE)) 
        {unElementalTicker.UnGreyOut();}
        if(IsThereStillACardOfThisClassInTheCurrentCollection(BeastClass.COMMON)) 
        {commonTicker.UnGreyOut();}
        ChangeCountText();
    }

   
    void WipeBeastDeck()
    {
        foreach (var item in currentCardStackDict)
        {
           Destroy(item.Value.gameObject) ;
        }
        currentCardStackDict.Clear();
    }

   
    
    public bool IsThereStillACardOfThisClassInTheCurrentCollection(BeastClass bc)
    {
        foreach (var item in catalogCardDict)
        {  
            if(!item.Value.dontCare)
            {
                if(item.Key.beastClass == bc)
                {return true;}
               
            }
        }
        return false;
    }

    public bool IsThereStillACardOfThisElementInTheCurrentCollection(Element e)
    {
        foreach (var item in catalogCardDict)
        {  
            if(!item.Value.dontCare)
            {
                if(item.Key.element == e)
                {return true;}
               
            }
        }
        return false;
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
            { currentCardStackDict[item].Stack();  }
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

    public void SortBeastButtons()
    {
        foreach (var item in beastButtonDict)
        {
            item.Value.transform.SetSiblingIndex(PlayerParty.inst.party.IndexOf(item.Key) );
            
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