using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public enum CoinResult{Heads,Tails,NA}
public class CoinManager : Singleton<CoinManager>,IPointerDownHandler
{
    public static CoinResult coinResult { get; private set; }
    public static bool canSpin;
    public bool playerSpinning;
    [SerializeField] RectTransform coin;
    [SerializeField] float spinSpeed;
    [SerializeField] float spinTime;
    [SerializeField] Image picture;
    [SerializeField] Sprite[] faces;
    [SerializeField] float farAway;
    [SerializeField] float close;
    [SerializeField] AudioSource flipSFX;
    [SerializeField] AudioSource landSound;
    bool spinning;
    float z;
    int index = 1;
    
    
    void OnEnable()
    {
        coinResult = CoinResult.NA;
        canSpin = false;
    }

    public void AllowSpin(){
        playerSpinning = true;
        canSpin = true;
    }
   
    public void OnPointerDown(PointerEventData eventData)
    {
        if(!spinning & canSpin)
        {Flip();}
    }

    public void Flip()
    {   
        flipSFX.Play();
        z = Random.Range(-180,180);
        spinning = true;
        Spin();
        GetCloser();
        StartCoroutine(PlaySoundAfterDelay());
    }

    IEnumerator PlaySoundAfterDelay(){
        yield return new WaitForSeconds(spinTime - spinSpeed/3);
         landSound.Play();
    }

    void Spin()
    {
        if(spinning)
        {
            if(index == 0)
            {index = 1;}
            else if(index == 1)
            {index = 0;}
            picture.sprite = faces[index];
           
            Quaternion i = Quaternion.Euler(180,0,z);
            Quaternion ii = Quaternion.Euler(0,0,z);
           
            coin.DOLocalRotateQuaternion(i,spinSpeed).OnComplete(()=> 
            { coin.DOLocalRotateQuaternion(ii,spinSpeed).OnComplete(()=> Spin());});        
        }
    }

    void GetCloser()
    {
        coin.DOScale(close,spinTime/2).OnComplete(() => coin.DOScale(farAway,spinTime/2).OnComplete(()=>EndSpin()));
    }

    void EndSpin()
    {
       
        spinning = false;
        canSpin = false;
        int result = Random.Range(0,2);
        picture.sprite = faces[result];
        spinning = false;
        if(result == 0)
        {coinResult = CoinResult.Heads;}
        else
        {coinResult = CoinResult.Tails;}
        BattleManager.inst.miniGameResultHandler.DisplayCoinResult(coinResult);
        if(playerSpinning)
        {
            UIManager.inst.SideCast();
        }
        else{
            EnemyAI.inst.ContineCoin();
        }

        playerSpinning = false;
     
      
    }

    
}