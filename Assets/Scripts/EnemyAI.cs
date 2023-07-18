using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyAI : Singleton<EnemyAI>
{
    public void Act()
    {
        StartCoroutine(q());
        IEnumerator q()
        {
            yield return new WaitForSeconds(.66f);
            Debug.Log("Bleh");
            BattleManager.inst.EndTurn();

        }
    }
    
}