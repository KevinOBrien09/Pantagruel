using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BeastSwapper : Singleton<BeastSwapper>
{
    public GameObject parent;
    public RectTransform rt;
    public Image buttonIcon;
    public Sprite X,swapIcon;
    public HealthBar healthBar;
    public bool open;
    public Button left,right,main,swap;
    float beastPicStart;
    public TextMeshProUGUI indexDisplay,beastName,healthValues;
    public Image[] beastPic;
    public TextMeshProUGUI passiveName,passiveDesc;
    int currentIndex;
    public int maxIndex;

    void Start()
    {
        beastPicStart = rt.anchoredPosition.x;
        gameObject.SetActive(false);
        open = false;
        buttonIcon.sprite = swapIcon;
        //ToggleMenu(); ///This makes no fucking sense but do not remove it.
    }
  
    public void ToggleMenu()
    {
        if(BattleManager.inst.turnState != BattleManager.TurnState.Player)
        {return;}

        CreateEntrys();
        if(open)
        {
            gameObject.SetActive(false);
            if(!BattleManager.inst.inBattle)
            {OverworldMovement.canMove = true;}
            buttonIcon.sprite = swapIcon;
            open = false;
        }
        else
        {
            gameObject.SetActive(true);
            // if(CardCatalog.inst.open)
            // {CardCatalog.inst. ToggleMenu();}
            if(!BattleManager.inst.inBattle)
            {OverworldMovement.canMove = false;}
            buttonIcon.sprite = X;
            open = true;
        }
    }

    void CreateEntrys()
    {
        maxIndex = PlayerParty.inst.party.Count;
        currentIndex = 1;
        indexDisplay.text = currentIndex.ToString() + "/" + maxIndex.ToString();

        if(currentIndex == 1)
        {left.gameObject.SetActive(false);}
        if(currentIndex == maxIndex)
        {right.gameObject.SetActive(false);}
        else
        {right.gameObject.SetActive(true);}

        ApplyBeastInfo();
    }

    public void DeactivateButton()
    {
        swap.gameObject.SetActive(false);
    }

    public void ReactivateButton()
    {
        swap.gameObject.SetActive(true);
    }



    public void ApplyBeastInfo()
    {
        BeastData so = PlayerParty.inst.party[currentIndex-1].scriptableObject.beastData;

        foreach (var item in beastPic)
        {item.sprite = so.mainSprite;}
        beastName.text = so.beastName;
        // healthValues.text =  PlayerParty.inst.party[currentIndex-1].currentHealth +"/"+ 
        // PlayerParty.inst.party[currentIndex-1].scriptableObject.beastData.stats.maxHealth; 
        healthBar.beast =  PlayerParty.inst.party[currentIndex-1];

        if(so.passive != null)
        {
            passiveName.text = "Passive: " +  so.passive.passiveName;
            passiveDesc.text = so.passive.passiveDesc;;
        }
        else
        {
            passiveDesc.text = "";
            passiveName.text = "Passive: NOT FOUND";
        }
        healthBar.UpdateFill();
        healthBar.UpdateText();
    }

    public void Swap()
    {
        if(PlayerParty.inst.activeBeast == PlayerParty.inst.party[currentIndex-1]){ //band aid
            Debug.Log("Beast is the same fuck off");
            return;
        }
        //ToggleMenu();
        if(!BattleManager.inst.inBattle)
        {OverworldMovement.canMove = true;}
        parent.SetActive(false);
        buttonIcon.sprite = swapIcon;
        open = false;
        
        Beast newActiveBeast = PlayerParty.inst.party[currentIndex-1];
        BottomCornerBeastDisplayer.inst.ChangeActiveBeast(newActiveBeast);
        PlayerParty.inst.ChangePartyOrder(newActiveBeast);
        PassiveManager.inst.OrginizePassiveActivity();
        if(BattleManager.inst.inBattle)
        {
            CardManager.inst.SwitchBeast(newActiveBeast);
            DeactivateButton();
            CardManager.inst.DeactivateHand();
            BattleManager.inst. StartCoroutine(q());
            IEnumerator q()
            {
                yield return new WaitForSeconds(.5f);
                BattleManager.inst.EndTurn();
            }
        }
        CreateEntrys();
    }
    
    public void MoveLeft()
    {
        currentIndex--;
        ApplyBeastInfo();
        
        rt.DOAnchorPosX(-1700,0);
        rt.DOAnchorPosX(beastPicStart,.5F);
  

        if(currentIndex == 1)
        {left.gameObject.SetActive(false);}
        right.gameObject.SetActive(true);
        indexDisplay.text = currentIndex.ToString() + "/" + maxIndex.ToString();
    }

    public void MoveRight()
    {
        currentIndex++;
        ApplyBeastInfo();
        rt.DOAnchorPosX(1700,0);
        rt.DOAnchorPosX(beastPicStart,.5F);
        
        if(currentIndex > 1)
        { left.gameObject.SetActive(true); }

        if(currentIndex == maxIndex)
        { right.gameObject.SetActive(false); }

        indexDisplay.text = currentIndex.ToString() + "/" + maxIndex.ToString();
        
    }

}
