using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAI : Singleton<EnemyAI>
{
    public Skill skillCache;
    Beast currentBeast;
    
    public void Go(Beast b)
    {StartCoroutine(Basic(b));}

    public void Pass()
    {
        MainTextTicker.inst.Type("Pass.");
        BattleManager.inst.StartCoroutine( BattleManager.inst.EndOfTurnDelay(2));
    }
    
    IEnumerator Basic(Beast b)
    {
        if(b != null)
        {
             if(b.data.skills.Count == 0)
            {
                Debug.LogAssertion( b.data.beastName +" has no skills");
                Pass();
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(1.3f);
                System.Random rnd = new System.Random();
                List<Skill> randomizedSkills = b.data.skills.OrderBy(item => rnd.Next()).ToList();
                Skill skill = null;
            
                foreach (var item in randomizedSkills)
                {
                    if(item.skillData.cantCastWithFullHealth)
                    {
                        if(b.currentHealth == b.data.stats.maxHealth)
                        {
                            continue;
                        }
                        else
                        {
                        skill = item;
                        break;
                        }
                    }
                    else{
                        skill = item;
                        break;
                    }
                }

                if(skill != null)
                {
                    MainTextTicker.inst.Type(skill.skillData.skillName+".");
                    skillCache = skill;
                    currentBeast = b;

                }
                else{
                    Debug.LogAssertion("No valid skill selected");
                    Pass();
                    yield break;
                }

                if(skill.skillData.miniGame == SkillMiniGame.Gauge)
                {
                    if(skill.skillData.miniGame == SkillMiniGame.Gauge){
                        GaugeTicker.inst.SetUp(skill.skillData.gaugeBullseyes);
                    }
                }
                
                SkillMinigameManager.inst.ChangeState(skillCache.skillData.miniGame,false);
                yield return new WaitForSeconds(.5f);

                switch (skillCache.skillData.miniGame)
                {
                    case SkillMiniGame.Basic:
                
                    StartCoroutine(Cast());
                    break;
                    case SkillMiniGame.Dice_DigitalRoot:
                    DiceManager.inst.Roll(false);
                    StartCoroutine(Dice());
                    break;
                    case SkillMiniGame.Coin_HeadTails:
                    StartCoroutine(Coin());
                    break;
                    case SkillMiniGame.Gauge:
                    StartCoroutine(Gauge()) ;
                    break;


                    default: Debug.LogAssertion("DefaultCase"); break;
                }

            }
            

        }
        else{
            Debug.LogAssertion("No beast in enemy AI");
        }
       
        
    }

    IEnumerator Gauge()
    {
        GaugeTicker.inst.Move();
        float d = Random.Range(2f,4f);
        yield  return new WaitForSeconds(d);
        GaugeTicker.inst.Stop();
        StartCoroutine(Cast(.1f));



    }
    

    IEnumerator Coin()
    {
        CoinManager.inst.playerSpinning = false;
        yield return new WaitForSeconds(.5f);
        BattleManager.inst.miniGameResultHandler.QuestionMark(false);
        CoinManager.inst.Flip();
    }
    
    public void ContineCoin()
    {StartCoroutine(Cast());}
    
    IEnumerator Dice()
    {
        BattleManager.inst.miniGameResultHandler.QuestionMark(false);
        yield return new WaitForSeconds(.1f);
        while( DiceManager.inst.index <  DiceManager.inst.dice.Count)
        {
            if(!DiceManager.inst.dice.ElementAtOrDefault(DiceManager.inst.index))
            {break;}
            yield return new WaitForSeconds(.25f);
            if( DiceManager.inst.dice.ElementAtOrDefault(DiceManager.inst.index))
            {DiceManager.inst.dice[DiceManager.inst.index].Land(false);}
            else {break;}
            yield return null;
        }
        StartCoroutine(Cast());
    }

    IEnumerator Cast(float delay = .5f)
    {
        float i = 0;
        // if(skillCache.skillData.miniGame == SkillMiniGame.Gauge){

        // }
        yield return new WaitForSeconds(delay);
        CastData cd;
        switch (skillCache.skillData.target)
        {
            case(Target.Opposition):

                cd = new CastData(
                caster_:currentBeast,
                target_:BattleManager.inst.activePlayerBeast,
                casterDigitalRoot: BattleManager.inst.miniGameResultHandler.digitalRoot);
                i = skillCache.delayTilNextTurn;
                if(skillCache.skillData.beastMoveType == BeastMoveType.Pounce)
                {
                    currentBeast.StartCoroutine(currentBeast.movementHandler.Pounce(skillCache.skillData.sound,skillCache,cd));
                }
                else if(skillCache.skillData.beastMoveType == BeastMoveType.Bounce)
                {   
                    currentBeast.StartCoroutine(currentBeast.movementHandler.Bounce(skillCache.skillData.sound,skillCache,cd));
                
                }
                else if(skillCache.skillData.beastMoveType == BeastMoveType.Spin)
                {   
                    currentBeast.StartCoroutine(currentBeast.movementHandler.Spin(skillCache.skillData.sound,skillCache,cd));
                
                }
                else if(skillCache.skillData.beastMoveType == BeastMoveType.None)
                {   
                    skillCache.Go(cd);
                }
          
          
            break;

            case(Target.Self):
                cd = new CastData(
                caster_:currentBeast,
                target_:currentBeast,
                casterDigitalRoot: BattleManager.inst.miniGameResultHandler.digitalRoot);
                i = skillCache.delayTilNextTurn;

                
                currentBeast.StartCoroutine(currentBeast.movementHandler.Bounce(skillCache.skillData.sound,skillCache,cd));
                
                
          //  Debug.Log("Enemy self cast not added");
            break;
            
            default:
            break;
        }

        yield return new WaitForSeconds(.25f);
        currentBeast = null;
        skillCache = null;
    
        BattleManager.inst.StartCoroutine( BattleManager.inst.EndOfTurnDelay(i));

    }
}
