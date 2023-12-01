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
    public GameObject stunBirds;
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
            newBeast.transform.localPosition = scriptableObject.beastData.battlePos;
            newBeast.layer =battleLayer;
            foreach (Transform item in newBeast.transform)
            {
                item.gameObject.layer = battleLayer;
            }

            BeastAnimatedInstance AI =   newBeast.AddComponent<BeastAnimatedInstance>();
            AI.Init(RivalBeastManager.inst.activeBeast);
            animatedInstance = AI;
            currentBeast = newBeast;
            if(AI.stunBirds == null){
               GameObject birds =  Instantiate(stunBirds,AI.transform);
                foreach (Transform item in MiscFunctions.GatherAllTransforms(birds.transform,new List<Transform>()))
                {
                    item.gameObject.layer = battleLayer;
                }
               birds.transform.localPosition = new Vector3(birds.transform.localPosition.x,
               scriptableObject.beastData.stunBirdHeight,birds.transform.localPosition.z);
               birds.SetActive(false);
               AI.stunBirds = birds;
            }

        }
        else
        {
            basicRenderer.enabled = true;
            basicRenderer.sprite = scriptableObject.beastData.mainSprite;

            BeastAnimatedInstance AI =   basicRenderer.gameObject.AddComponent<BeastAnimatedInstance>();
            AI.Init(RivalBeastManager.inst.activeBeast);
            animatedInstance = AI;
            if(AI.stunBirds == null){
               GameObject birds =  Instantiate(stunBirds,AI.transform);
                foreach (Transform item in MiscFunctions.GatherAllTransforms(birds.transform,new List<Transform>()))
                {
                    item.gameObject.layer = battleLayer;
                }
               birds.transform.localPosition = new Vector3(birds.transform.localPosition.x,
               scriptableObject.beastData.stunBirdHeight,birds.transform.localPosition.z);
               birds.SetActive(false);
               AI.stunBirds = birds;
            }
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
