using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StatusEffectManager:Singleton<StatusEffectManager>
{
    [SerializeField] GameObject statusEffectPrefab;
    [SerializeField] Transform holder;
    [SerializeField] Transform enemyHolder;

    public StatusEffectMonoBehaviour MakeIcon(StatusEffectInstance i)
    {
        GameObject g = Instantiate(statusEffectPrefab,holder);
        StatusEffectMonoBehaviour mb = g.GetComponent<StatusEffectMonoBehaviour>();
        mb.Apply(i,Alliance.Player);
        return mb;
    }

    public StatusEffectMonoBehaviour MakeEnemyIcon(StatusEffectInstance i)
    {
        GameObject g = Instantiate(statusEffectPrefab,enemyHolder);
        StatusEffectMonoBehaviour mb = g.GetComponent<StatusEffectMonoBehaviour>();
        mb.Apply(i,Alliance.Enemy);
        return mb;
    }

    public void UpdateIcon()
    {

    }
    
}