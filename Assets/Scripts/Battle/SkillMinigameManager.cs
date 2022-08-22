using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class MiniGameStates
{
    public RectTransform rt;
    public Vector2 pos;

}

public enum SkillMiniGame{Hidden,Basic,Dice_DigitalRoot,Coin_HeadTails,Gauge}
public class SkillMinigameManager : Singleton<SkillMinigameManager>
{
    public Vector2 hidden;
    Dictionary<SkillMiniGame,MiniGameStates> miniGameDict = new Dictionary<SkillMiniGame, MiniGameStates>();
    public MiniGameStates Dice_DR;
    public MiniGameStates Coin_HT;
    public MiniGameStates Hidden;
    public MiniGameStates basicState;
    public MiniGameStates gauge;
    public MiniGameStates currentDisplay;
    float s =.2f;

    protected override void Awake()
    {
        base.Awake();
        miniGameDict.Add(SkillMiniGame.Hidden,Hidden);
        miniGameDict.Add(SkillMiniGame.Dice_DigitalRoot,Dice_DR);
        miniGameDict.Add(SkillMiniGame.Coin_HeadTails,Coin_HT);
        miniGameDict.Add(SkillMiniGame.Basic,basicState);
        miniGameDict.Add(SkillMiniGame.Gauge,gauge);
        
        foreach(var item in miniGameDict)
        {item.Value.rt.DOAnchorPos(hidden,0);}
        miniGameDict[SkillMiniGame.Hidden].rt.DOAnchorPos(miniGameDict[SkillMiniGame.Hidden].pos,.2f);
        currentDisplay = miniGameDict[SkillMiniGame.Hidden];
        //ChangeState(SkillMiniGame.Hidden,false);
    }

    public bool ChangeState(SkillMiniGame miniGame,bool player)
    { 
        if(miniGame == SkillMiniGame.Basic){
            return true;   
        }
        if(currentDisplay != miniGameDict[miniGame]) //if current state not same as incoming state
        {
            if(currentDisplay != miniGameDict[SkillMiniGame.Hidden]) // if the current state is not hidden do stuff
            {
                MiniGameStates smg = miniGameDict[miniGame];
                currentDisplay.rt.DOAnchorPos(hidden,s).OnComplete(() => smg.rt.DOAnchorPos(smg.pos,s).OnComplete(()=> SkillCaster.inst.StartMiniGame(miniGame,player)));
                currentDisplay = smg;
            }
            else // if the state is hidden
            {
                MiniGameStates smg = miniGameDict[miniGame];
                currentDisplay.rt.DOAnchorPos(hidden,s);
                smg.rt.DOAnchorPos(smg.pos,s).OnComplete(()=> SkillCaster.inst.StartMiniGame(miniGame,player));
                currentDisplay = smg;
            }

            if(player)
            {
                switch (miniGame)
                {
                    case SkillMiniGame.Basic:
                    BattleManager.inst.miniGameResultHandler.ChangeHeader("Basic");
                    BattleManager.inst.miniGameResultHandler.QuestionMark(false);

                    break;

                    case SkillMiniGame.Dice_DigitalRoot:
                    BattleManager.inst.miniGameResultHandler.ChangeHeader("Digital Root");
                    BattleManager.inst.miniGameResultHandler.QuestionMark(true);
                    break;

                    case SkillMiniGame.Coin_HeadTails:
                    BattleManager.inst.miniGameResultHandler.ChangeHeader("CoinFlip");
                    BattleManager.inst.miniGameResultHandler.QuestionMark(true);
                    break;

                    case SkillMiniGame.Hidden:
                    BattleManager.inst.miniGameResultHandler.ChangeHeader("Hidden");
                    BattleManager.inst.miniGameResultHandler.QuestionMark(true);
                    break;

                    case SkillMiniGame.Gauge:
                    BattleManager.inst.miniGameResultHandler.ChangeHeader("Gauge2");
                    BattleManager.inst.miniGameResultHandler.QuestionMark(true);
                      GaugeTicker.inst.Reset();

                    break;


                    default:
                    Debug.LogAssertion("Default case");
                    break;
                }
            }
            else
            {
                switch (miniGame)
                {
                    case SkillMiniGame.Basic:
                    BattleManager.inst.miniGameResultHandler.ChangeHeader("Basic");
                    BattleManager.inst.miniGameResultHandler.QuestionMark(false);
                    break;

                    case SkillMiniGame.Dice_DigitalRoot:
                    BattleManager.inst.miniGameResultHandler.ChangeHeader("Digital Root");
                    BattleManager.inst.miniGameResultHandler.QuestionMark(false);
                    break;

                    case SkillMiniGame.Coin_HeadTails:
                    BattleManager.inst.miniGameResultHandler.ChangeHeader("CoinFlip");
                    BattleManager.inst.miniGameResultHandler.QuestionMark(false);
                    break;

                    case SkillMiniGame.Hidden:
                    BattleManager.inst.miniGameResultHandler.ChangeHeader("Hidden");
                    BattleManager.inst.miniGameResultHandler.QuestionMark(false);
                    break;

                    case SkillMiniGame.Gauge:
                    BattleManager.inst.miniGameResultHandler.ChangeHeader("Gauge2");
                    BattleManager.inst.miniGameResultHandler.QuestionMark(false);
                      GaugeTicker.inst.Reset();

                    break;
                    
                    default:
                    Debug.LogAssertion("Default case");
                    break;
                }
            }
         
            return true;
        }
        else
        {
            return false;
          
        }
      
    
    }
}
