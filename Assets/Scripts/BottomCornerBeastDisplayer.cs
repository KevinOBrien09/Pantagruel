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
    public TextMeshProUGUI beastName,beastLevel,lvlText;
    public HealthBar healthBar;
    public Image bg;
    GameObject animatedPrefabInstance;
    Beast beast;
    Dictionary<Beast,BeastAnimatedInstance> instances = new Dictionary<Beast, BeastAnimatedInstance>();

    public void ToggleBattleBGOn(){
        bg.enabled = true;
    }
     public void ToggleBattleBGOff(){
        bg.enabled = false;
    }

    public void CreateAnimatedInstances(List<Beast> beasts)
    {
        foreach (var b in beasts)
        {   
            if(!instances.ContainsKey(b))
            {
                if(b.scriptableObject.beastData.ANIMATED_PREFAB_DONE)
                {
                    GameObject g =  Instantiate(b.scriptableObject.beastData.beastGraphicPrefab,animatedPrefabHolder);
                    g.transform.localPosition = b.scriptableObject.beastData.bottomCornerPos;
                    BeastAnimatedInstance AI =   g.AddComponent<BeastAnimatedInstance>();
                    AI.Init(b);
                    animatedInstance = AI;
                    g.SetActive(false);
                
                        instances.Add(b,AI);
                    
                    
                } 
                else
                {
                    SpriteRenderer g =  Instantiate(basicRenderer,animatedPrefabHolder);
                    g.sprite = b.scriptableObject.beastData.mainSprite;
                    g.transform.localPosition = b.scriptableObject.beastData.bottomCornerPos;
                    BeastAnimatedInstance AI =   g.gameObject. AddComponent<BeastAnimatedInstance>();
                    AI.Init(b);
                    animatedInstance = AI;
                    g.gameObject.SetActive(false);
                    instances.Add(b,AI);
                }
            }

        }
    }

    public void NoBeast(){
        
        beastName.text = "";
        //beastLevel.text = "";
        healthBar.gameObject.SetActive(false);
        //lvlText.gameObject.SetActive(false);
        healthBar.fill.fillAmount = 0;
        healthBar.current.text = "0";
        // healthBar.max.text = "0";
        // healthBar.shieldFill.fillAmount = 0;
    }

    public void YesBeast(){
        healthBar.gameObject.SetActive(true);
        //lvlText.gameObject.SetActive(true);
    }

    public void ChangeActiveBeast(Beast b,bool playAnim)
    {
        beastName.text = "Lvl."+ b.exp.level.ToString() + " " + b.scriptableObject.beastData.beastName;
        healthBar.entity = b;
       // beastLevel.text = ;
        b.currentHealthBars.Add(healthBar);
        healthBar.onInit.Invoke();
        beast = b;
       CardManager.inst.manaHandler.SwapBeast(PlayerParty.inst.activeBeast);
        if(playAnim)
        {
            instances[b].ToggleSpringBones(false);
            animatedPrefabHolder.DOMoveX(5,.2f).OnComplete(()=>
            {
                toggle();
                animatedPrefabHolder.DOMoveX(0,.33f);
            });
        }
        else
        {toggle();}
      
        void toggle()
        {
            foreach (var item in instances)
            {item.Value.gameObject.SetActive(false);}
            instances[b].gameObject.SetActive(true);
            instances[b].ToggleSpringBones(true);
            animatedPrefabHolder.DOMoveX(0,.33f);
        }
      
    }

    public void RefreshLevel(){
        beastLevel.text = beast.exp.level.ToString();
        healthBar.onInit.Invoke();
    }

}