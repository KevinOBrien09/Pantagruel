using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Torch : MonoBehaviour
{
    public bool torchOn;
    [SerializeField] RectTransform torch,parent;
    Vector2 torchOG;
    [SerializeField]  float torchStepMultipler = 5;
    [SerializeField] float torchReady,torchDown;
    [SerializeField] Image meterFill;
    [SerializeField] TextMeshProUGUI durationText;
    public float maxSteps,currentSteps;
    bool left;
    
    void Awake()
    {
        torchOG = torch.anchoredPosition;
       currentSteps = maxSteps;
        parent.DOAnchorPosY(torchDown,0);
        meterFill.fillAmount = currentSteps/maxSteps;
        durationText.text =currentSteps + "Steps.";
    }

    public void Load(bool torchState,float timer)
    {
       currentSteps = timer;

        if(torchOn)
        {
            parent.DOAnchorPosY(torchReady,0);
        }
        else
        {
            parent.DOAnchorPosY(torchDown,0);
        }
    }

    void Update(){
        // if(currentSteps <= 0){
        //     DisableTorch();
        // }

        if(!BattleManager.inst.inBattle){
            if(InputManager.input.torch)
            {
                if(torchOn)
                {
                    DisableTorch();
                }
                else
                {
                    EnableTorch();
                }
            }
        }

    }

   public void DeductStep(){
        currentSteps--;
         meterFill.DOFillAmount(currentSteps/maxSteps,.1f);
         //fillAmount = ;
        durationText.text =currentSteps + " Steps.";
             if(currentSteps <= 0){
            DisableTorch();
            
        }
    }

    
    public void EnableTorch()
    {
        if(currentSteps > 0)
        {
            parent.DOAnchorPosY(torchReady,.5f);
            torchOn = true;
        }
    }

    public void DisableTorch()
    {
        parent.DOAnchorPosY(torchDown,.5f);
        torchOn = false;
    }

    public void Bounce()
    {
        if(torchOn)
        {
            if(left)
            {
                torch.DOAnchorPos(new Vector2(torchOG.x-torchStepMultipler,torchStepMultipler),.2f).OnComplete(()=> torch.DOAnchorPos(new Vector2(torchOG.x,0),.2f));
                left = false;
            }
            else
            {
                torch.DOAnchorPos(new Vector2(torchOG.x+torchStepMultipler,torchStepMultipler),.2f).OnComplete(()=> torch.DOAnchorPos(new Vector2(torchOG.x,0),.2f));
                left = true;

            }
        }}
}
