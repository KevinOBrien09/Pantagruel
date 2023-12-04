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
    [SerializeField] SoundData data;
    public GameObject realLight;
    Vector3 realLightOG;
    public float maxSteps,currentSteps;
    bool left;
    
    void Awake()
    {
        torchOG = torch.anchoredPosition;
       currentSteps = maxSteps;
        parent.DOAnchorPosY(torchDown,0);
        meterFill.fillAmount = currentSteps/maxSteps;
        durationText.text =currentSteps + "Steps.";
        realLightOG = realLight.transform.localPosition;
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
            AudioManager.inst.GetSoundEffect().Play(data);
            parent.DOAnchorPosY(torchReady,.5f).OnComplete(()=>{
 realLight.SetActive(true);
            });
           
            torchOn = true;
        }
    }

    public void DisableTorch()
    {
        parent.DOAnchorPosY(torchDown,.5f);
        realLight.SetActive(false);
        torchOn = false;
    }

    public void Bounce()
    {
        float realLightMult = .25f;
        if(torchOn)
        {
            if(left)
            {
                torch.DOAnchorPos(new Vector2(torchOG.x-torchStepMultipler,torchStepMultipler),.2f).OnComplete(()=> torch.DOAnchorPos(new Vector2(torchOG.x,0),.2f));
                realLight.transform.DOLocalMove(new Vector3(realLightOG.x-realLightMult,realLightOG.y- realLightMult,realLightOG.z), .2f);
                left = false;
            }
            else
            {
                torch.DOAnchorPos(new Vector2(torchOG.x+torchStepMultipler,torchStepMultipler),.2f).OnComplete(()=> torch.DOAnchorPos(new Vector2(torchOG.x,0),.2f));
                 realLight.transform.DOLocalMove(new Vector3(realLightOG.x+realLightMult,realLightOG.y-realLightMult,realLightOG.z), .2f);
                left = true;

            }
        }}
}
