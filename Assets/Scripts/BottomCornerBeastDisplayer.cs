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
    public GameObject animatedInstance;
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
            animatedInstance = g;
            g.transform.localPosition = b.scriptableObject.beastData.bottomCornerPos;
            //g.layer = playerBeastLayer;
            basicRenderer.enabled = false;
        } 
        else
        {
            basicRenderer.enabled = true;
            basicRenderer.sprite = b.scriptableObject.beastData.mainSprite;
            basicRenderer.transform.localPosition = b.scriptableObject.beastData.bottomCornerPos;
        }

        beastName.text = b.scriptableObject.beastData.beastName;
        healthBar.beast = b;
        healthBar.onInit.Invoke();
    }

}