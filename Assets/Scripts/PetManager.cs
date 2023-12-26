
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PetManager:Singleton<PetManager>                   
{
    public PetBehaviour enemyPet,playerPet,petPrefab;
    public Transform enemyPetHolder,playerPetHolder;

  
    public void SummonPlayerPet(Pet p)
    {
       if(playerPet!=null){
            //kill
        }
        EventManager.inst.onPetSummon.Invoke();
        EventManager.inst.onPlayerPetSummon.Invoke();
        AudioManager.inst.GetSoundEffect().Play(p.summoned);
        playerPet = Instantiate(petPrefab,playerPetHolder);
        playerPet.gameObject.layer = 8;
        playerPet.Init(p,EntityOwnership.PLAYER);
        playerPet.transform.GetChild(0).gameObject.layer = 8;
      
        BattleManager.inst.SetPlayerTarget(playerPet);
        PlayerParty.inst.activeBeast.animatedInstance.MoveBack(EntityOwnership.PLAYER);
        BattleField.inst.Summon(p,playerPet,EntityOwnership.PLAYER);

    }   

    public void SummonEnemyPet(Pet p)
    {
        if(enemyPet!=null){
            //kill
        }
        BattleManager.inst.FUCKOFF = true;
        EventManager.inst.onPetSummon.Invoke();
        EventManager.inst.onEnemyPetSummon.Invoke();
        AudioManager.inst.GetSoundEffect().Play(p.summoned);
        enemyPet = Instantiate(petPrefab,enemyPetHolder);
        enemyPet.gameObject.layer = 7;
 
        enemyPet.Init(p,EntityOwnership.ENEMY);
        enemyPet.transform.GetChild(0).gameObject.layer = 7;
        //RivalBeastManager.inst.healthBar.SummonPetHealthBar(enemyPet);
        BattleManager.inst.SetEnemyTarget(enemyPet);
        RivalBeastManager.inst.activeBeast.animatedInstance.MoveBack(EntityOwnership.ENEMY);
        BattleField.inst.Summon(p,enemyPet,EntityOwnership.ENEMY);
        StartCoroutine(q());
        IEnumerator q(){
            yield return new WaitForSeconds(.7f);
            BattleManager.inst.FUCKOFF = false;
        }
    }

    public void KillPet(PetBehaviour p)
    {
        //EventManager.inst.onPetDeath.Invoke();
        if(p == enemyPet)
        {
           
            StartCoroutine(q());
            IEnumerator q()
            {
                CardManager.inst.DeactivateHand();
           BattleManager.inst.FUCKOFF = true;
                yield return new WaitForSeconds(1);
      
                //RivalBeastManager.inst.healthBar.entity = null;
                       BattleField.inst.enemyPet.healthBar.entity = null;
                //RivalBeastManager.inst.healthBar.RemovePetHealthBar();
                 BattleField.inst.enemyPet.gameObject.SetActive(false);
                 if(enemyPet!=null){
                    Destroy(enemyPet.gameObject);
                 }
            
               
                enemyPet = null;
                BattleManager.inst.SetEnemyTarget(RivalBeastManager.inst.activeBeast);
                RivalBeastManager.inst.activeBeast.animatedInstance.MoveForward(EntityOwnership.ENEMY);
                yield return new WaitForSeconds(.7f);
             //   CardManager.inst.ActivateHand();
                BattleManager.inst.FUCKOFF = false;
            }
        }
        else if(p == playerPet)
        {    
            StartCoroutine(q());
            IEnumerator q()
            {
                BattleManager.inst.SetPlayerTarget(PlayerParty.inst.activeBeast);
              
                yield return new WaitForSeconds(1);
           
               
                BattleField.inst.playerPet.gameObject.SetActive(false);
                BattleField.inst.playerPet.healthBar.entity = null;
                if(playerPet !=null){
                    Destroy(playerPet.gameObject);
                }
                
                playerPet = null;
              
                PlayerParty.inst.activeBeast.animatedInstance.MoveForward(EntityOwnership.PLAYER);
            }
              
        }
    }

    public void LeaveBattle()
    {
        if(playerPet != null){
            
            playerPet.Die(EntityOwnership.ERROR);
        }
    }


}