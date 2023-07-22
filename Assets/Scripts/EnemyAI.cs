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
    public List<ManaGem> activeGems = new List<ManaGem>();
    public TextMeshProUGUI manaCountText;
    public GameObject cardBack;
    public RectTransform cardBackHolder;
    Dictionary<int,GameObject> backDict = new Dictionary<int, GameObject>();
    
    public List<Card> discardPile = new List<Card>();
    
    public void InitNewBeast(Beast b){
        currentDeck.cards = new List<Card>(b.deck.cards);

    }

    public void Act(Beast b)
    {
      
        DrawCard();

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
                    discardPile.Add(usedCard);
                    SpendMana(usedCard.manaCost);
                    
                    foreach (var effect in usedCard.effects)
                    {effect.Use(RivalBeastManager.inst.currentBeast,PlayerManager.inst.party.activeBeast);}
                    
                }

                RebuildCardBacks();
                yield return new WaitForSeconds(.5f);
                BattleManager.inst.EndTurn();
            }
        }
    }

    public void DrawCard()
    {
        if(currentDeck.cards.Count <= 0)
        {
            Debug.Log("No cards in enemy deck");
            return;
        }

        if(hand.Count < 7)
        {
            Card c = currentDeck.cards[Random.Range(0,currentDeck.cards.Count)];
            currentDeck.cards.Remove(c);
            hand.Add(c);
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
    
}