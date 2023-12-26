using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class EnemyAI : Singleton<EnemyAI>
{
    
    public Deck currentDeck;
    public List<Card> hand = new List<Card>();
    public GameObject cardBack;
    public RectTransform cardBackHolder;
    public ManaHandler manaHandler;
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
            // Debug.Log("Resetting discard pile");
            return;
        }
        
        if(hand.Count < 7)
        {
          
            Card c = CardFunctions.DrawRandomCard(currentDeck);
            hand.Add(c);
              RebuildCardBacks();
        }
    }

    public void Act(Beast b)
    {
        DrawRandomCard();
        

        StartCoroutine(q());
        IEnumerator q()
        { 
            yield return new WaitForSeconds(.33f);
            EventManager.inst.onEnemyDrawCard.Invoke();
            RebuildCardBacks();
            CardCombo chosenCombo = GetCombo(b);
            yield return new WaitForSeconds(.5f);
            if(chosenCombo == null)
            {
                BattleTicker.inst.Type("Turn skip");
                yield return new WaitForSeconds(1);
                BattleManager.inst.EndTurn();
            }
            else
            {  
                // Debug.Log("Has a combo do turn");
                
               
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

                Queue<Card> c = new Queue<Card>();
                foreach (var usedCard in toBeUsed)
                {
                    c.Enqueue(usedCard);
                   
                }
            
            peepoo:
                if(c.Count != 0)
                {
                    
                    Card usedCard = c.Dequeue();
                    BattleManager.inst.enemyRecord[BattleManager.inst.turn].cardsPlayedThisTurn.Add(usedCard);
                   
                    BattleTicker.inst.Type(usedCard.cardName);
                    yield return new WaitForSeconds(.25f);
                   
                    hand.Remove(usedCard);
                    currentDeck.discard.Add(usedCard);
                    manaHandler.Spend(usedCard.manaCost);
                    EventManager.inst.onEnemyCastCard.Invoke();
                    
                    if(usedCard.soundEffect.audioClip != null)
                    {AudioManager.inst.GetSoundEffect().Play(usedCard.soundEffect); }
                
                    bool dodged = Maths.PercentCalculator(PlayerParty.inst.activeBeast.dodge);
                    if(usedCard.unDodgeable)
                    {
                        dodged = false;
                    }
                    CardStackBehaviour cardStackBehaviour =   CardStack.inst.CreateActionStack(usedCard,RivalBeastManager.inst.activeBeast,dodged);

                    foreach (var effect in usedCard.effects)
                    {
                        EffectArgs args = new EffectArgs(RivalBeastManager.inst.activeBeast,PlayerParty.inst.activeBeast, usedCard,cardStackBehaviour,
                        BattleManager.inst.enemyRecord[BattleManager.inst.turn].cardsPlayedThisTurn.IndexOf(usedCard),usedCard.cardName);
                        effect.Use(args);
                    }
                  

                    RebuildCardBacks();
                    yield return new WaitForSeconds(.5f);
                    goto peepoo;
                }
                else
                {
                    yield return new WaitForSeconds(.5f);
               
                    BattleManager.inst.EndTurn();
                }
            }
        }
    }


    CardCombo GetCombo(Beast b)
    {
        DeckObject wildDeck = b.scriptableObject.beastData.wildDeck;
        List<CardCombo> validCombos = new List<CardCombo>();
        foreach (var combo in wildDeck.combos)
        {
           // bool hasCardsForCombo = combo.requiredCards.All(comboCard => hand.Any(handCard => handCard == comboCard));
            if(hasCardsForCombo(combo))
            { 
                Debug.Log("1");
                if(manaHandler.currentMana >= combo.fullManaCost())
                {    Debug.Log("2");
                    bool allCardsAreCastable = true;
                    foreach (var item in combo.requiredCards)
                    {
                        if(!CardFunctions.canCast(item,false))
                        {allCardsAreCastable = false;}
                         
                    }

                    if(allCardsAreCastable)
                    {validCombos.Add(combo);
                     Debug.Log("4");}
                }
            }

            Debug.Log(validCombos.Count +" valid combos");
        }

      
        if(validCombos.Count > 0)
        {
        return  validCombos[Random.Range(0,validCombos.Count)];
        }
        else
        {
            return null;
        }
       
    }

    bool hasCardsForCombo(CardCombo c)
    {
        bool b = true;
        foreach (var comboCard in c.requiredCards)
        {
            if(!hand.Contains(comboCard))
            {
                b = false;
                break;
            }
        }


        return b;
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

  

    public void LeaveBattle(){
        currentDeck = null;
       
       
        deckDict.Clear();
       

         hand.Clear();
        RebuildCardBacks();
        backDict.Clear();
       
      
    }
}