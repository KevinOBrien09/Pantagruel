using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster:Singleton<GameMaster>
{

    public enum GameState{Overworld,Battle,Menu}
    public GameState currentGameState;
    void Start(){
        SkillMinigameManager.inst.StartingMiniGame(SkillMiniGame.CharacterFace);
        ChangeGameState(GameState.Overworld);
    }

    public void ChangeGameState(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.Overworld:
            OverWorldManager.inst.SwapToOverworld();
            break;
            case GameState.Battle:
            OverWorldManager.inst.SwapToBattle();
            break;
            
           
        }
        currentGameState = newGameState;

    }

    
    

}