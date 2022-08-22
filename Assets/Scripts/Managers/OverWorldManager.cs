using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverWorldManager:Singleton<OverWorldManager>
{
    [SerializeField] GameObject enemyHealth;
    [SerializeField] GameObject playerSprite;
    [SerializeField] Animator cameraBob;

   
    public void SwapToOverworld()
    {
        MainTextTicker.inst.Type("Exploration.");
        enemyHealth.SetActive(false);
        playerSprite.SetActive(true);
        cameraBob.enabled = false;

    }


}