using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStack : Singleton<CardStack>
{
    public GameObject history,stack;
    public Transform historyStack,stackStack;
    public List<GameObject> currentTurn = new List<GameObject>();
    public CardStackBehaviour cardStackBehaviour;
    public CardStackBehaviour levelStack;
    bool stackState;
    //Quest IDEA bandit betrayal. 

    public void Swap()
    {
        stackState = !stackState;
        if(stackState)
        {
            stack.SetActive(true);
            history.SetActive(false);
        }
        else{
            stack.SetActive(false);
            history.SetActive(true);
        }

    }

    public void CreateActionStack(Card c,Beast b)
    {
        CardStackBehaviour action = Instantiate(cardStackBehaviour,stackStack);
        action.Init(c,b);
        currentTurn.Add(action.gameObject);

    }

    public void CreateHistoryStack(Card c,Beast b)
    { 
        CardStackBehaviour histAction = Instantiate(cardStackBehaviour,historyStack);
        histAction.Init(c,b);
    }

    public void CreateTurnCounter()
    {
        CardStackBehaviour action = Instantiate(levelStack,historyStack); 
        action.actionName.text = action.actionName.text + "0";
    }
}
