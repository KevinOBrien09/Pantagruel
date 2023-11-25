using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardStackBehaviour : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI cardName,actionResult,meterValues;
    public Image cardPic,bg,meterFill;
    public Color32 blue,red;
    public Card savedCard;
    public Beast caster;
    public GameObject promiseParent;
    public int turnPlayed;
    public GameObject meterGO;
   
    
    public void Init(Card card,Beast b,bool isPlayer)
    {
        if(isPlayer){
            bg.color = blue;
        }
        else{
            bg.color = red;
        }
        cardName.text = card.cardName;
        cardPic.sprite = card.picture;

        savedCard = card;
        caster = b;
        turnPlayed = BattleManager.inst.turn;
        Promise p = null;
       
        if (card.effects .OfType<Promise>().Any())
        {
            foreach (var item in card.effects)
            {
                if(item.GetType() == typeof(Promise)|item.GetType() == typeof(DamagePromise) )
                {
                    p = (Promise)item;
                    break;
                }
                
            }
            if(p.meter){
                actionResult.enabled = false;
                meterGO.SetActive(true);
                UpdateBar(0,p.meterMax);
            }
            else{
                ConditionPending();
            }
           
            promiseParent.gameObject.SetActive(true);
        }
        else
        {
            promiseParent.gameObject.SetActive(false);
        }
    }

    public void UpdateBar(float current,float max){
        meterFill.DOFillAmount(current/max,.2f);
        if(current > max)
        {current = max;}
        meterValues.text = current.ToString() + "/" + max.ToString();
    }

    public void ConditionPending(){
    actionResult.text = "<color=grey><i>Condition Pending";
    }

    public void ConditionFufilled(){
         meterGO.SetActive(false);
        actionResult.enabled = true;
    actionResult.text = "<color=green><i>Condition Fufilled!";
    }

    public void ConditionFailed(){
        meterGO.SetActive(false);
        actionResult.enabled = true;
    actionResult.text = "<color=red> Condition Failed.";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!BattleManager.inst.inBattle){
            return;
        }
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            List<Card> cards = new List<Card>();
         
            cards.Add(savedCard);
            CardViewer.inst.manuallyLoadCards = true;
            CardManager.inst.DeactivateHand();
            CardViewer.inst.ManuallyLoadCards(cards);
            CardViewer.inst.Open(savedCard);
        }
       
    }
  
}
