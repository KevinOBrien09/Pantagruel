using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectDisplay : MonoBehaviour
{
    public StatusEffectHandler statusEffectHandler;
    public List<GameObject> graphics = new List<GameObject>();
    public int turnToDie;
    public int turnCastOn;
    public StatusEffects statusEffect;
    public StatusEffectEffect scriptableObject;
    public List<Card> correspondingCards = new List<Card>();
    public LookAt lookAt;
    string enemyCam = "BattleCamera";
    string playerCam = "BottomCornerCam";

    public void Init(StatusEffects so,StatusEffectHandler handler)
    {
        statusEffectHandler = handler;
        switch(so)
        {
            case StatusEffects.POISON:
            graphics[0].SetActive(true);
            scriptableObject = statusEffectHandler. allEffects[0];
            break;
            case StatusEffects.SHIELD:
            graphics[1].SetActive(true);
          
            scriptableObject = statusEffectHandler. allEffects[3];
            break;
            case StatusEffects.PARASITE:
            graphics[2].SetActive(true);
            scriptableObject =  statusEffectHandler.allEffects[1];
            break;
            case StatusEffects.HOLYWATER:
            graphics[3].SetActive(true);
            scriptableObject = statusEffectHandler. allEffects[2];
            break;
         
            case StatusEffects.WOUND:
            graphics[4].SetActive(true);
            scriptableObject = statusEffectHandler. allEffects[4];
            break;
            
            default:
            Debug.LogAssertion("Default Case");
            break;
        }
        if(statusEffectHandler.owner.OwnedByPlayer() ){
            lookAt.cameraName = playerCam;
        }else{
            lookAt.cameraName = enemyCam;
        }


        turnCastOn = BattleManager.inst.turn;
        
        statusEffect = so;
        
        
    }

    public void Trigger()
    {
       
        scriptableObject.Trigger(statusEffectHandler.owner,statusEffectHandler.owner.OwnedByPlayer());
    }

    public void AddStatusEffectCardToDeck(List<Card> cards)
    {
        if(statusEffectHandler.owner.OwnedByPlayer()){
            foreach (var item in cards)
            {
                statusEffectHandler. owner.deck.AddCardToDeck(item);
                correspondingCards.Add(item);
                statusEffectHandler.owner.deck.Shuffle();
            }
           
        }
        else
        { foreach (var item in cards)
            {
                EnemyAI.inst.currentDeck.AddCardToDeck(item);
                correspondingCards.Add(item);
                EnemyAI.inst.currentDeck.Shuffle();
            }
        }
    }


    public void RemoveCards()
    {
        // foreach (var item in  correspondingCards)
        // {
        //     if(BattleManager.inst.GetBeastOwnership( statusEffectHandler. owner) == EntityOwnership.PLAYER)
        //     {CardManager.inst.currentDeck.DestroyCard(item);}
        //     else
        //     {EnemyAI.inst.currentDeck.DestroyCard(item);}
        // }
        // correspondingCards.Clear();
    }

    public void Kill(){
        RemoveCards();
        statusEffectHandler.currentEffects.Remove(statusEffect);
        statusEffectHandler.displays.Remove(this);
        Destroy(this.gameObject);
    }
}