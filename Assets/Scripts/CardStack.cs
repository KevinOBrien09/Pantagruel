using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoganeUnityLib;
using DG.Tweening;
public class CardStack : Singleton<CardStack> //holder for card history 
{

    public Transform holder;
    public TMP_Typewriter turnCounter;
    public List<CardStackBehaviour> behaviours = new List<CardStackBehaviour>();
    public List<TMP_Typewriter> turns = new List<TMP_Typewriter>();
    Dictionary<int,List<Card>> turnActionDict = new Dictionary<int, List<Card>>();
    public CardStackBehaviour cardStackBehaviour;
    public Scrollbar scrollbar;
    bool stackState;
    //Quest IDEA bandit betrayal. 

    public void NewTurn()
    {  int lastTurn = BattleManager.inst.turn-1;
          if(turnActionDict[lastTurn].Count == 0){
           TMP_Typewriter q =  Instantiate(turnCounter,holder);
           
           turns.Add(q);
        }
        TMP_Typewriter typewriter =  Instantiate(turnCounter,holder);
      
        typewriter.Play("Turn " + lastTurn.ToString(),50,(()=>{}));
        DOVirtual.Float( scrollbar.value,1,.2f,v  => 
        {  scrollbar.value = v;});
        turnActionDict.Add(BattleManager.inst.turn,new List<Card>());
        turns.Add(typewriter);
      
    }
    public void EnterBattle()
    {
      TMP_Typewriter typewriter =  Instantiate(turnCounter,holder);
          turns.Add(typewriter);
      typewriter.Play("Battle Start",50,(()=>{}));
        DOVirtual.Float( scrollbar.value,1,.2f,v  => 
        {  scrollbar.value = v;});
        turnActionDict.Add(1,new List<Card>());
    }

    public void ExitBattle()
    {
        TMP_Typewriter typewriter =  Instantiate(turnCounter,holder);
        turns.Add(typewriter);
        typewriter.Play("Battle End",50,(()=>{}));
        DOVirtual.Float( scrollbar.value,1,.2f,v  => 
        {  scrollbar.value = v;});
        turnActionDict.Clear();
     
    }


    public CardStackBehaviour CreateActionStack(Card c,Beast b,bool isPlayer)
    {
        CardStackBehaviour action = Instantiate(cardStackBehaviour,holder);
        action.Init(c,b,isPlayer);
        behaviours.Add(action);
        turnActionDict[BattleManager.inst.turn].Add(c);
        DOVirtual.Float( scrollbar.value,1,.2f,v  => 
        {  scrollbar.value = v;});

        return action;
    }

    public void Wipe()
    {
        foreach (var item in behaviours)
        { Destroy(item.gameObject); }

        foreach (var item in turns)
        { Destroy(item.gameObject); }

        behaviours.Clear();
        turns.Clear();
    }    

   
}
