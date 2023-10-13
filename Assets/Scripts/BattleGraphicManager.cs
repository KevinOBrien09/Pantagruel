using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGraphicManager : Singleton<BattleGraphicManager>
{
    public GameObject currentBeast;
    public Transform holder;
    public int battleLayer;
    public SpriteRenderer basicRenderer;
    public BeastAnimatedInstance animatedInstance;
    public void Init(BeastScriptableObject scriptableObject)
    {
        if(currentBeast != null)
        { 
            Destroy(currentBeast); 
        }
            
        if(scriptableObject.beastData.ANIMATED_PREFAB_DONE)
        {
            basicRenderer.enabled = false;
            GameObject newBeast = Instantiate(scriptableObject.beastData.beastGraphicPrefab,holder);
            newBeast.transform.position = scriptableObject.beastData.battlePos;
            newBeast.layer =battleLayer;
            foreach (Transform item in newBeast.transform)
            {
                item.gameObject.layer = battleLayer;
            }

            BeastAnimatedInstance AI =   newBeast.AddComponent<BeastAnimatedInstance>();
            AI.Init(RivalBeastManager.inst.activeBeast);
            animatedInstance = AI;
            currentBeast = newBeast;

        }
        else
        {
            basicRenderer.enabled = true;
            basicRenderer.sprite = scriptableObject.beastData.mainSprite;

            BeastAnimatedInstance AI =   basicRenderer.gameObject.AddComponent<BeastAnimatedInstance>();
            AI.Init(RivalBeastManager.inst.activeBeast);
            animatedInstance = AI;
        }
    }

    public void Wipe()
    {
        Destroy(animatedInstance);
        if(currentBeast != null)
        {Destroy(currentBeast);}
        currentBeast = null; 
        animatedInstance = null;
    
        basicRenderer.enabled = false;
        basicRenderer.material.SetFloat("_DissolvePower",1);
    }
}
