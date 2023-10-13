using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class EnemyAI : Singleton<EnemyAI>
{
    public int maxMana;
    public int currentMana;
    public int maxManaBase = 8;
    public Deck currentDeck;
    public List<Card> hand = new List<Card>();
    public List<ManaGem> gems = new List<ManaGem>();
    public List<ManaGem>  activeGems = new List<ManaGem>();
    public TextMeshProUGUI manaCountText;
    public GameObject cardBack;
    public RectTransform cardBackHolder;
    Dictionary<int,GameObject> backDict = new Dictionary<int, GameObject>();
    Dictionary<Beast,Deck> deckDict = new Dictionary<Beast, Deck>();
    
    public void CreateDecks(List<Beast> b)
    {
        foreach (var item in b)
        {
            deckDict.Add(item,new Deck());
            deckDict[item].cards = new List<Card>(item.deck.cards);
        }
    }
    
    public void SwapActiveBeast(Beast newActiveBeast)
    {
        foreach (var item in backDict)
        {Destroy(item.Value.gameObject); }
        foreach (var item in hand)
        { currentDeck.discard.Add(item); }
        backDict.Clear();
        currentDeck = deckDict[newActiveBeast];
    }

    public void  DrawRandomCard()
    {
        if(currentDeck.cards.Count <= 0)
        {
            currentDeck.ResetDiscardPile();
            Debug.Log("Resetting discard pile");
            return;
        }
        
        if(hand.Count < 7)
        {
          
            Card c = CardFunctions.DrawRandomCard(currentDeck);
            hand.Add(c);
        }
    }

    public void Act(Beast b)
    {
        DrawRandomCard();
        DeckObject wildDeck = b.scriptableObject.beastData.wildDeck;
        List<CardCombo> validCombos = new List<CardCombo>();
        foreach (var combo in wildDeck.combos)
        {
            bool hasCardsForCombo = combo.requiredCards.All(comboCard => hand.Any(handCard => handCard == comboCard));
            if(hasCardsForCombo)
            { validCombos.Add(combo); }
        }

        StartCoroutine(q());
        IEnumerator q()
        { 
            yield return new WaitForSeconds(.33f);
            EventManager.inst.onEnemyDrawCard.Invoke();
            RebuildCardBacks();
            yield return new WaitForSeconds(.5f);
            if(validCombos.Count == 0)
            {
                BattleTicker.inst.Type("Turn skip");
                yield return new WaitForSeconds(1);
                BattleManager.inst.EndTurn();
            }
            else
            {  
                Debug.Log("Has a combo do turn");
                CardCombo chosenCombo = validCombos[Random.Range(0,validCombos.Count)];
                List<Card> toBeUsed = new List<Card>();
                Dictionary<int,bool> checkList = new Dictionary<int, bool>();
               
                for (int i = 0; i < chosenCombo.requiredCards.Count; i++)
                {checkList.Add(i,false);}
                
                for (int i = 0; i < checkList.Count; i++)
                {
                    if(checkList[i])
                    { continue; }
                    else
                    {
                        foreach (var cardInHand in hand)
                        {
                            if(cardInHand == chosenCombo.requiredCards[i] && !toBeUsed.Contains(cardInHand))
                            {
                                toBeUsed.Add(cardInHand);
                         
                                break;
                            }
                        }
                    }
                }

                foreach (var usedCard in toBeUsed)
                {
                    hand.Remove(usedCard);
                    currentDeck.discard.Add(usedCard);
                    SpendMana(usedCard.manaCost);
                    EventManager.inst.onEnemyCastCard.Invoke();
                    BattleTicker.inst.Type(usedCard.cardName);
                    
                    foreach (var effect in usedCard.effects)
                    {effect.Use(RivalBeastManager.inst.activeBeast,PlayerParty.inst.activeBeast);}
                }

                RebuildCardBacks();
                yield return new WaitForSeconds(.5f);
                BattleManager.inst.EndTurn();
            }
        }
    }
    
    public void RebuildCardBacks()
    {
        foreach (var item in backDict)
        {Destroy(item.Value);}
        backDict.Clear();
        backDict = new Dictionary<int, GameObject>();

        for (int i = 0; i < hand.Count; i++)
        {
            GameObject g = Instantiate(cardBack,cardBackHolder);
            backDict.Add(i,g);
        }
    }

    public void IncreaseMaxMana()
    {
        if(maxManaBase != maxMana)
        {
            maxMana++;

            for (int i = 0; i < maxMana; i++)
            {
                if(!gems[i].active)
                {
                    gems[i].Activate();
                    activeGems.Add(gems[i]);
                }
            }
        }
    }

    public void SpendMana(int manaCost)
    {
        currentMana = currentMana- manaCost;
        UpdateDisplay();
    }
    
    public void RegenMana()
    {
        currentMana = maxMana;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        foreach (var item in activeGems)
        {item.TurnOffCircle();}

        for (int i = 0; i < currentMana; i++)
        {activeGems[i].TurnOnCircle();}

        manaCountText.text = currentMana + "/" + maxMana;
    }

    public void LeaveBattle(){
        currentDeck = null;
        maxMana =0;
        currentMana = 0;
        foreach (var item in gems)
        {item.Deactivate();}
       
        deckDict.Clear();
       
        activeGems.Clear();
         hand.Clear();
        RebuildCardBacks();
        backDict.Clear();
       
        UpdateDisplay();
    }
}