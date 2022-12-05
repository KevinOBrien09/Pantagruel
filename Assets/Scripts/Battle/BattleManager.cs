using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager:Singleton<BattleManager>
{
    public enum BattleState{Start,PlayerTurn,EnemyTurn,Win,Lose}
    public enum BattleEndCause{Continue,Win,Lose,Draw,Swap}
    public BattleState currentBattleState;
    public Beast activePlayerBeast;
    public Beast activeEnemyBeast;
    public MiniGameResultHandler miniGameResultHandler;
    public SkillHandler skillHandler;
    public DamageSplash splash;
    public SkillCaster caster;
    public List<Beast> enemyBeasts = new List<Beast>();
    public BeastDisplay beastDisplay;
    public EnemyStatusEffectManager enemyStatusEffect;
    [SerializeField] HealthBar playerBeastHealthbar;
    [SerializeField] SPBar sPBar;
    [SerializeField] RivalBeastSpawner rivalBeastSpawner;
    [SerializeField] EnemyAI enemyAI;
    [SerializeField] AudioSource drumSound;
    public int turn;
    bool battleMusic;
    public bool callBM;
    [SerializeField] bool NODEATH;
    

    protected override void Awake()
    {
         base.Awake();
       
    }
    public void CreateEnemy(){
        enemyBeasts = rivalBeastSpawner.Spawn();
        activeEnemyBeast = enemyBeasts[0];
        activeEnemyBeast.allience = Alliance.Enemy;
        activeEnemyBeast.MakeActive();
    }
  
    public void StartBattle()
    {
        skillHandler.DeActivateSkillButtons();
        currentBattleState = BattleState.Start;
        MainTextTicker.inst.Type("The Game begins!");
        NewTurn();
    }
    
    IEnumerator OnBattleStart()
    {
        yield return new WaitForSeconds(1);
        currentBattleState = BattleState.EnemyTurn;
        NewTurn();
    }
    
    public void ApplyPlayerBeastInfo(Beast b)
    {
        skillHandler.InitNewSkills(b.data);
        beastDisplay.Apply(b);
        playerBeastHealthbar.Apply(b);
        sPBar.Apply(b);
    }
    
    public void ApplyEnemyBeastInfo(Beast b)
    {
        
    }
    
    public IEnumerator EndOfTurnDelay(float i = 1)
    {
        yield return new WaitForSeconds(i);
      NewTurn() ;
    }

    public IEnumerator PassTurn()
    {
        MainTextTicker.inst.Type("Pass.");
        yield return new WaitForSeconds(1);
        StartCoroutine(EndOfTurnDelay());
    }
    
    public void NewTurn()
    {
       BattleEndCause bec = BattleCanContinue();
        PlayerInventory.inst.UpdateBeastStatus();
        if(bec == BattleEndCause.Continue)
        {
        //   drumSound.Play();
            turn++;
            switch (currentBattleState )
            {
                case BattleState.Start:
                StartCoroutine(OnBattleStart()) ;
                break;

                case BattleState.PlayerTurn: 
              
                if(TickUpdate(activeEnemyBeast))
                {StartCoroutine(Tick(activeEnemyBeast));}
                else
                {DirectlySwapToEnemy();}
                break;
                
                case BattleState.EnemyTurn:
                List<bool> b = new List<bool>();
                for (int i = 0; i < PlayerInventory.inst.beasts.Count; i++)
                {
                    if(PlayerInventory.inst.beasts[i].isDead == false){
 b.Add(true);
                    }
                   
                }
                Debug.Log(b.Count);
                if(b.Count >= 2){
  UIManager.inst.ReactivateSwapBeastButton();
                }
              
                if(TickUpdate(activePlayerBeast))
                {StartCoroutine(Tick(activePlayerBeast));}
                else
                {DirectlySwapToPlayer();}

                break;
                
                default:
                Debug.LogAssertion("Default Switch");
                break;
            }
        }
        else if (bec == BattleEndCause.Swap)
        {
            Debug.Log("Swap");
            UIManager.inst.Swap();
        }
    }

    IEnumerator Tick(Beast b) //Make this based on a queue.
    {
        if(b.allience == Alliance.Player)
        {
            SkillMinigameManager.inst.ChangeState(SkillMiniGame.BeastFace,false);
           
            MainTextTicker.inst.Type("Bleed");
            yield return new WaitForSeconds(.5f);  
            b.statusEffectHandler.Tick();
            yield return new WaitForSeconds(.75f);
            if(BattleCanContinue() == BattleEndCause.Continue)
            {
                MainTextTicker.inst.Type("Make your move.");
                UIManager.inst.SideNormal();
                skillHandler.ReActivateSkillButtons(activePlayerBeast);
                currentBattleState = BattleState.PlayerTurn;
            }  
        }
        else
        {   
            skillHandler.DeActivateSkillButtons();
            MainTextTicker.inst.Type("Bleed");
            if(SkillDisplay.inst.gameObject.activeSelf)
            {SkillDisplay.inst.StartCoroutine(SkillDisplay.inst.Hide());}
            currentBattleState = BattleState.EnemyTurn;

            yield return new WaitForSeconds(.5f);  
            b.statusEffectHandler.Tick();
            if(BattleCanContinue() == BattleEndCause.Continue)
            {enemyAI.Go(activeEnemyBeast);}
        }
    }

    public void DirectlySwapToPlayer()
    {
      
        activePlayerBeast.statusEffectHandler.Tick();
        MainTextTicker.inst.Type("Make your move.");
        skillHandler.ReActivateSkillButtons(activePlayerBeast);
        SkillMinigameManager.inst.ChangeState(SkillMiniGame.BeastFace,false);
        UIManager.inst.SideNormal();
        currentBattleState = BattleState.PlayerTurn;
    }

    public void DirectlySwapToEnemy()
    {
      
        activeEnemyBeast.statusEffectHandler.Tick();
        MainTextTicker.inst.Type("The beast is afoot!");
        enemyAI.Go(activeEnemyBeast);
    
        skillHandler.DeActivateSkillButtons();
        if(SkillDisplay.inst.gameObject.activeSelf)
        {SkillDisplay.inst.StartCoroutine(SkillDisplay.inst.Hide());}
        currentBattleState = BattleState.EnemyTurn;

    }



    public BattleEndCause BattleCanContinue()
    {
        if(NODEATH)
        {
            return BattleEndCause.Continue;
        }
        else
        {
           
            if(activePlayerBeast.currentHealth <= 0 && activeEnemyBeast.currentHealth <= 0)
            {
                StartCoroutine(Draw());
                 Reset();
                return BattleEndCause.Draw;
            }
            else if(activePlayerBeast.currentHealth <= 0)
            {
                List<bool> b = new List<bool>();
                for (int i = 0; i < PlayerInventory.inst.beasts.Count; i++)
                {
                    b.Add( PlayerInventory.inst.beasts[i].isDead);
                }

                if(b.Contains(false))
                {
                    MainTextTicker.inst.Type("Send out new beast!");
                    activePlayerBeast = null;
                    callBM = true;
                    return BattleEndCause.Swap;
                }
                else
                {
                    StartCoroutine(Lose());
                    Reset();
                    return BattleEndCause.Lose;
                }
             
               
            }
            else if (activeEnemyBeast.currentHealth <= 0)
            {
                StartCoroutine(Win());
                 Reset();
                return BattleEndCause.Win;
            }
            else
            {return BattleEndCause.Continue;}

        }
      
    }

    IEnumerator Win()
    {
        
    	MainTextTicker.inst.Type("Victory.");
        yield return new WaitForSeconds(2);
        MusicManager.inst.ChangeToDungeon();
        Debug.LogWarning("Add level up here");
        OverWorldManager.inst.s();
        ///GameMaster.inst.ChangeGameState(GameMaster.GameState.Overworld);

    }

    IEnumerator Lose()
    {
        yield return new WaitForSeconds(.5f);
        MainTextTicker.inst.Type("Defeat.");
        yield return new WaitForSeconds(.5f);
        FadeToBlack.inst.Fade(2);
        yield return new WaitForSeconds(2.1f);
        Debug.Log("Stuff goes here");
    }

    IEnumerator Draw()
    {
        yield return new WaitForSeconds(1);
        MainTextTicker.inst.Type("Draw.");
    }

    public void BattleMusic(){
        if(!battleMusic){
            MusicManager.inst.ChangeToBattle();
            battleMusic = true;
        }

    }

    public void Reset()
    {
        battleMusic = false;
        activeEnemyBeast = null;
    }



    public bool TickUpdate(Beast b)
    {
        foreach (var item in b.statusEffectHandler.dict)
        {
            foreach (var e in item.Value.statusEffectInstances)
            {
                foreach (var a in e.effects)
                {
                    if(a.GetType() == typeof(Bleed))
                    {return true;}
                }
                
            }
        }
        return false;
    }

  
  
}