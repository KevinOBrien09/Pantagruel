using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BottomCornerBeastDisplayer: Singleton<BottomCornerBeastDisplayer>
{
    public Transform animatedPrefabHolder;
    public SpriteRenderer basicRenderer;    
    public BeastAnimatedInstance animatedInstance;
    public TextMeshProUGUI beastName;
    public HealthBar healthBar;

    public void ChangeActiveBeast(Beast b)
    {
        if(animatedInstance!= null)
        {
            Destroy(animatedInstance);
        }

        if(b.scriptableObject.beastData.ANIMATED_PREFAB_DONE)
        {
            GameObject g =  Instantiate(b.scriptableObject.beastData.beastGraphicPrefab,animatedPrefabHolder);
            g.transform.localPosition = b.scriptableObject.beastData.bottomCornerPos;
            BeastAnimatedInstance AI =   g.AddComponent<BeastAnimatedInstance>();
            AI.Init(b);
            animatedInstance = AI;
            basicRenderer.enabled = false;
            //g.layer = playerBeastLayer;
        } 
        else
        {
            basicRenderer.enabled = true;
            basicRenderer.sprite = b.scriptableObject.beastData.mainSprite;
            basicRenderer.transform.localPosition = b.scriptableObject.beastData.bottomCornerPos;

            BeastAnimatedInstance AI =   basicRenderer.gameObject.AddComponent<BeastAnimatedInstance>();
            AI.Init(b);
            animatedInstance = AI;
        }

        beastName.text = b.scriptableObject.beastData.beastName;
        healthBar.beast = b;
        b.currentHealthBar = healthBar;
        healthBar.onInit.Invoke();
    }

}