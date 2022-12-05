using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverWorldManager:Singleton<OverWorldManager>
{
    [SerializeField] GameObject enemyHealth;
    [SerializeField] GameObject playerSprite;
    [SerializeField] Animator cameraBob;
    [SerializeField] AudioSource battleStartBang;

    [SerializeField] AudioSource camDash;
   
    public void SwapToOverworld()
    {
        SkillMinigameManager.inst.ChangeState(SkillMiniGame.CharacterFace,true);
        MainTextTicker.inst.Type("Exploration.");
        UIManager.inst.MainInfoAreaSwap(UIManager.MainDisplayAreaState.TextInput);
        enemyHealth.SetActive(false);
        playerSprite.SetActive(true);
        cameraBob.enabled = false;

    }

    public void SwapToBattle()
    {
        StartCoroutine(b());
    }

    IEnumerator b()
    {
       
        MainTextTicker.inst.Type("Ambushed!");
        battleStartBang.pitch = Random.Range(.9f,1.1f);
        battleStartBang.Play();
        MainCameraManager.inst.RotateX(0);
        FadeViewportToBlack.inst.UnFade(.1f);
        DungeonManager.inst.KillRoom();
        BattleManager.inst.CreateEnemy();
        BattleManager.inst.BattleMusic();
        DungeonManager.inst.MakeEncounterRoom();
        playerSprite.SetActive(false);
        enemyHealth.SetActive(true);
        MainCameraManager.inst.ChangeZPos(-20,0);
        yield return new WaitForSeconds(.7f);
         SkillMinigameManager.inst.ChangeState(SkillMiniGame.BeastFace,true);
        BattleManager.inst.StartBattle();
        camDash.Play();
           UIManager.inst.SideNormal();
        MainCameraManager.inst.ChangeZPos(-10,.1f);
        UIManager.inst.MainInfoAreaSwap(UIManager.MainDisplayAreaState.Battle);
        cameraBob.enabled = true;
    }

    public void s(){
        StartCoroutine(ContinueToRoom());
    }

    public IEnumerator ContinueToRoom(){
        DungeonManager.inst.ContinueToRoom();
   
     
        yield return new WaitForSeconds(.5f);
        MainTextTicker.inst.Type("Exploration.");
        UIManager.inst.MainInfoAreaSwap(UIManager.MainDisplayAreaState.TextInput);
        SkillMinigameManager.inst.ChangeState(SkillMiniGame.CharacterFace,true);
        UIManager.inst.SideNormal();
        enemyHealth.SetActive(false);
        playerSprite.SetActive(true);
        cameraBob.enabled = false;

    }


}