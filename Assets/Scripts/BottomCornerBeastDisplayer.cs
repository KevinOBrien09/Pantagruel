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
    public TextMeshProUGUI beastName,beastLevel;
    public HealthBar healthBar;
    public Image bg;
    GameObject animatedPrefabInstance;
    Beast beast;

    public void ToggleBattleBGOn(){
        bg.enabled = true;
    }
     public void ToggleBattleBGOff(){
        bg.enabled = false;
    }
    public void ChangeActiveBeast(Beast b)
    {
        if(animatedPrefabInstance!= null) 
        {
            Destroy(animatedPrefabInstance);
        }

        if(b.scriptableObject.beastData.ANIMATED_PREFAB_DONE)
        {
            GameObject g =  Instantiate(b.scriptableObject.beastData.beastGraphicPrefab,animatedPrefabHolder);
            g.transform.localPosition = b.scriptableObject.beastData.bottomCornerPos;
            BeastAnimatedInstance AI =   g.AddComponent<BeastAnimatedInstance>();
            AI.Init(b);
            animatedInstance = AI;
            basicRenderer.enabled = false;
            animatedPrefabInstance = AI.gameObject;
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
        healthBar.entity = b;
        beastLevel.text = b.exp.level.ToString();
        b.currentHealthBars.Add(healthBar);
        healthBar.onInit.Invoke();
        beast = b;
    }

    public void RefreshLevel(){
 beastLevel.text = beast.exp.level.ToString();
     healthBar.onInit.Invoke();
    }

}