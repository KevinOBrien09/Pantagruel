using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillCaster : Singleton<SkillCaster>
{
    public Skill skillToBeCast;
    public DiceHolderMover mover;

    public void Precast(Skill s)
    {
        if(CheckSkillCost.canCast(BattleManager.inst.activePlayerBeast,s))
        {
            if(UIManager.inst.currentUIState == UIManager.UIState.Normal)
            {  
                if(s.skillData.cost.resource == ResourceCurrency.SP){
                    BattleManager.inst.activePlayerBeast.On_SP_Drain(s.skillData.cost.amount);
                }
                else{
                    BattleManager.inst.activePlayerBeast.IgnoreArmour(s.skillData.cost.amount,false);
                }
                
                MainTextTicker.inst.Type(s.skillData.skillName+".");
                BattleManager.inst.skillHandler.DeActivateSkillButtons();
                if(s.skillData.miniGame == SkillMiniGame.Dice_DigitalRoot 
                ||s.skillData.miniGame == SkillMiniGame.Coin_HeadTails )
                {BattleManager.inst.miniGameResultHandler.QuestionMark(true);}
                else
                {BattleManager.inst.miniGameResultHandler.QuestionMark(false);}
                var peepee = s;
                skillToBeCast = peepee;
                if(!SkillMinigameManager.inst.ChangeState(s.skillData.miniGame,true))
                {RealMiniGameStart(s.skillData.miniGame);}

                if(s.skillData.miniGame == SkillMiniGame.Gauge){
                    GaugeTicker.inst.SetUp(s.skillData.gaugeBullseyes);
                }

                if(s.skillData.miniGame == SkillMiniGame.Basic)
                {RealMiniGameStart(SkillMiniGame.Basic);}
            }
        }
        else
        {
            Debug.LogAssertion("Skill was cast but not enough resource, this should not happen");
            BattleManager.inst.NewTurn();
        }
    }

    public void StartMiniGame(SkillMiniGame miniGame,bool player)
    {
        if(miniGame == SkillMiniGame.Basic){
            return;
        }
        // else if(miniGame == SkillMiniGame.Coin_HeadTails){
        //        UIManager.inst.SideHide();
        //        return;
        // }
        if(player)
        {StartCoroutine(StartDelay(miniGame));}
    }

    IEnumerator StartDelay(SkillMiniGame miniGame)
    {
        
        yield return new WaitForSeconds(.25f);
        RealMiniGameStart(miniGame);
    }

    void RealMiniGameStart(SkillMiniGame miniGame)
    {
        switch (miniGame)
        {
            case SkillMiniGame.Basic:
            UIManager.inst.SideCast();
            break;
            case SkillMiniGame.Dice_DigitalRoot:
            DiceManager.inst.Roll(true);
            break;
            case SkillMiniGame.Coin_HeadTails:
         
            CoinManager.inst.AllowSpin();
            break;

            case SkillMiniGame.Gauge:
          
            GaugeTicker.inst.Move();
            UIManager.inst.SideCast();
            break;            
            default: Debug.LogAssertion("DefaultCase"); break;
        }
    
    }
    
    public void Cast()
    {
        if(skillToBeCast.skillData.miniGame == SkillMiniGame.Gauge)
        {
            GaugeTicker.inst.Stop();
        // return;
        }

        if(skillToBeCast != null)
        {
            if(skillToBeCast.skillData.target == Target.Opposition){
            float i =   skillToBeCast.Go(
            new CastData(
            caster_:BattleManager.inst.activePlayerBeast,
            target_:BattleManager.inst.activeEnemyBeast,
            casterDigitalRoot: BattleManager.inst.miniGameResultHandler.digitalRoot,
            diceMover_ :mover,
            effectGraphic_:skillToBeCast.skillData.effectGraphic
            )
            );
            UIManager.inst.SideHide();
           // UIManager.inst.Normal();
            BattleManager.inst.StartCoroutine(BattleManager.inst.EndOfTurnDelay(i));
            }
            else{
            float i =   skillToBeCast.Go(
            new CastData(
            caster_:BattleManager.inst.activePlayerBeast,
            target_:BattleManager.inst.activePlayerBeast,
            casterDigitalRoot: BattleManager.inst.miniGameResultHandler.digitalRoot,
            diceMover_ :mover,
            effectGraphic_:skillToBeCast.skillData.effectGraphic
            )
            );
            UIManager.inst.SideHide();
           // UIManager.inst.Normal();
            BattleManager.inst.StartCoroutine(BattleManager.inst.EndOfTurnDelay(i));
            }
           
        }
        else
        {
            Debug.LogAssertion("Skill to be cast is null" + transform.name);
        }
       

    }




}