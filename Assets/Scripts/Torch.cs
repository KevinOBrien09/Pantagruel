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
    public float maxDuration,currentCountDown;
    bool left;
    
    void Awake()
    {
        torchOG = torch.anchoredPosition;
        currentCountDown = maxDuration;
        parent.DOAnchorPosY(torchDown,0);
        meterFill.fillAmount = currentCountDown/maxDuration;
        durationText.text =(currentCountDown % maxDuration).ToString("00");
    }

    public void Load(bool torchState,float timer)
    {
        torchOn = torchState;
        currentCountDown = timer;
        meterFill.fillAmount = currentCountDown/maxDuration;
        durationText.text =(currentCountDown % maxDuration).ToString("00");

        if(torchOn)
        {
            parent.DOAnchorPosY(torchReady,0);
        }
        else
        {
            parent.DOAnchorPosY(torchDown,0);
        }
    }

    void Update()
    {
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

            if(torchOn)
            {
                if(currentCountDown > 0)
                {
                    currentCountDown -= Time.deltaTime;
                    meterFill.fillAmount = currentCountDown/maxDuration;
                    durationText.text =(currentCountDown % maxDuration).ToString("00");
                }
                else
                {
                    DisableTorch();
                }
            }
        }
        else
        {
            if(torchOn)
            {DisableTorch();}
        }
       
    }

    public void EnableTorch()
    {
        if(currentCountDown != 0)
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
