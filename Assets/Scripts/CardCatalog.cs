using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardCatalog : Singleton<CardCatalog>
{
    public GameObject slot;
    public Transform holder;
    public List<GameObject> activeCatalogCards = new List<GameObject>();
    public List<BeastClass> filteredClasses = new List<BeastClass>();
    public CardCatalogCard cardPrefab;

    void Start()
    {
        // List<Element> q = new List<Element>();
        // List<BeastClass> d = new List<BeastClass>();
        //  CreateCards(q,d);
    }
    public void Wipe()
    {
        foreach (var item in activeCatalogCards)
        {Destroy(item.gameObject);}

        activeCatalogCards.Clear();
    }

    public bool EditFilter(Element e,BeastClass beastClass)
    {
        Wipe();
        if(filteredClasses.Contains(beastClass)){
            filteredClasses.Remove(beastClass);
            CreateCards(new List<Element>(),filteredClasses);
            return false;
        }
        else{
            filteredClasses.Add(beastClass);
            CreateCards(new List<Element>(),filteredClasses);
            return true;
        }
       
    }

    public void FilterMassEdit(List<Element> e, List<BeastClass> b)
    {
        Wipe();
        filteredClasses.Clear();
        filteredClasses = new List<BeastClass>(b);
        CreateCards(new List<Element>(),filteredClasses);

    }
  

    public void CreateCards(List<Element> elements,List<BeastClass> filteredClasses)
    {
        //Show every card regardless of if it's owned or not.
        //  foreach (var card in BeastBank.inst.cardBank)
        //     {
        //         CardCatalogCard c =  Instantiate(cardPrefab,holder);
        //         c.Init(card);
        //         activeCatalogCards.Add(c.gameObject);
        //     }
        
        if(elements.Count == 0 && filteredClasses.Count == 0)
        {


            //empty thing setactive



            // List<Card> cards;
            // if(DeckEditor.inst.deckEditorState == DeckEditorState.ALLCARDS)
            // {
            //     cards = new List<Card>(CardCollection.inst.ownedCards);
            // }
            // else
            // {
            //     CreateCards(elements,DeckEditor.inst.GetBeastClasses(DeckEditor.inst.beastWhosDeckIsBeingEdited));
            //     return;
            // }

            // foreach (var card in cards)
            // {
            //     CardCatalogCard c =  Instantiate(cardPrefab,holder);
            //     c.Init(card);
            //     activeCatalogCards.Add(c.gameObject);
            // }
            //do everything
        }
        else
        {
            List<Card> validCards = new List<Card>();
            foreach (var card in BeastBank.inst.cardBank)
            {
                for (int i = 0; i < filteredClasses.Count; i++)
                {
                    if(filteredClasses.Contains(card.restrictions.classRestriction) )
                    {validCards.Add(card);
                    break;}
                    
                }
            }

            foreach (var card in validCards)
            {
                CardCatalogCard c =  Instantiate(cardPrefab,holder);
                c.Init(card);
                activeCatalogCards.Add(c.gameObject);
            }
        }
    }

    //Arcane/Trickster are filtered
    //We want solo Arcane solo Trickster and Arcane+Trickster only
}