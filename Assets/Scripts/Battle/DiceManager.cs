using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DiceManager : Singleton<DiceManager>
{
   public List<Die> dice = new List<Die>();
   public Die spinningDie;
   public int index;
  

    void Start()
    {
        index = -1;
       
    }

    public void Roll(bool p)
    {
        index++;
        if(index < dice.Count)
        {
            spinningDie = dice[index];
            spinningDie.StartCoroutine(spinningDie.Spin());
            //StartCoroutine(InputDelay());
        }
        else
        {
            spinningDie = null;
            End(p);
        }
    }

    void Update()
    {
        if(BattleManager.inst.currentBattleState == BattleManager.BattleState.PlayerTurn){
            Input();
        }

    }

 

    void Input()
    {
        if(InputManager.input.space)
        {
            spinningDie?.Land(true);
         
        }
    }

    void End(bool p)
    {
        index = -1;
        Debug.Log("done");
        List<string> numbersAsString = new List<string>();
        foreach (var item in dice)
        {
            int i = item.number+1 ;
            numbersAsString.Add(i.ToString());
        }
        
        BattleManager.inst.miniGameResultHandler.CalculateDigitalRoot(numbersAsString);
        if(p)
        {UIManager.inst.SideCast();}
      
    }


    public int DamageAmount(Beast b,int digitalRoot)
    {
        int damage = 0;

        List<int> dieResult = new List<int>();
        Dictionary<Die,HitResult> bleh = new Dictionary<Die, HitResult>();
        foreach (var item in dice)
        {
           dieResult.Add(item.number) ;
        }
        int i = -1;
        foreach (var item in dieResult)
        {
            i++;
            foreach (var p in b.data.pips)
            {
                if(item == p.number)
                {
                    if(p.result == HitResult.Hit)
                    {damage = damage+digitalRoot;
                    bleh.Add(dice[i],HitResult.Hit);
                    break;}
                    else if(p.result == HitResult.Crit)
                    {damage = damage + digitalRoot * 2;
                    bleh.Add(dice[i],HitResult.Crit);
                    break;}
                    else if (p.result == HitResult.Miss)
                    {damage = damage + 0;
                    bleh.Add(dice[i],HitResult.Miss);
                    break;}

                }
               
            }
        }
        StartCoroutine(ShowResults(bleh));
        return damage;
    }

    IEnumerator ShowResults(Dictionary<Die,HitResult> bleh)
    {
        yield return new WaitForSeconds(1);
        foreach (var item in bleh)
        {
            item.Key.DisplayHitResult(item.Value);
        }

    }
}
