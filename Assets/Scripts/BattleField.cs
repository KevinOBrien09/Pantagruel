using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class BattleField : Singleton<BattleField>
{
   

    public EntityIcon playerBeast,enemyBeast,playerPet,enemyPet;
    public List<float> yPositions = new List<float>();

    void Start(){
        enemyPet.gameObject.SetActive(false);
        playerPet.gameObject.SetActive(false);
    }
    public void Init()
    {
      
 
        enemyBeast.Move(yPositions[1]);
        playerBeast.Move(yPositions[2]);
    }

   public void SetPlayerBeastIcon(Beast b){
        playerBeast.InitBeast(b);
    }

    public void SetEnemyBeastIcon(Beast b){        
        enemyBeast.InitBeast(b);
    }

    public void Summon(Pet pet,PetBehaviour behaviour,EntityOwnership ownership)
    {
        if(ownership == EntityOwnership.PLAYER)
        {
           
            playerBeast.Move(yPositions[3]);
            StartCoroutine(q());
            IEnumerator q(){
                yield return new WaitForSeconds(.3f);
                playerPet.gameObject.SetActive(true);
                playerPet.InitPet(behaviour);
                playerPet.rt.anchoredPosition = new Vector2(0,yPositions[2]);
            }
        }
        else{
           enemyBeast.Move(yPositions[0]);
            StartCoroutine(q());
            IEnumerator q(){
                yield return new WaitForSeconds(.3f);
                enemyPet.gameObject.SetActive(true);
                enemyPet.InitPet(behaviour);
                enemyPet.rt.anchoredPosition = new Vector2(0,yPositions[1]);
            }
        }
    
       
    }



}