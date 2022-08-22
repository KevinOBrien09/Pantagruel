using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro;
using DG.Tweening;

public class Die : MonoBehaviour,IPointerClickHandler
{
    public List<Sprite> sprites = new List<Sprite>();
    [SerializeField] Image picture;
    [SerializeField] AudioSource spinSound;
    [SerializeField] AudioSource landSound;
    [SerializeField] RectTransform hitResultRT;
    [SerializeField] TextMeshProUGUI hitResultText;
    [SerializeField] float hitResultY;
    Dictionary<int,Sprite> faces = new Dictionary<int, Sprite>();
    public int number;
    bool spinning;
    float ogY;
    
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            var n = i;
            faces.Add(n,sprites[n]);
        }
        ApplyGraphic(5);
        ogY = hitResultRT.anchoredPosition.y;
    }  

    public void OnPointerClick(PointerEventData eventData)
    {
        if(BattleManager.inst.currentBattleState == BattleManager.BattleState.PlayerTurn)
        {
           Land(true);
        }
    }

    public void DisplayHitResult(HitResult hr)
    {   
        switch (hr)
        {
            case HitResult.Hit: hitResultText.text = "Hit"; hitResultText.color = Color.white; break;
            case HitResult.Miss:hitResultText.text = "Miss"; hitResultText.color = Color.white;break;
            case HitResult.Crit:hitResultText.text = "Crit"; hitResultText.color = Color.red;break;
            default: Debug.LogAssertion("Default Case"); hitResultText.text = "???";break;
        }

        hitResultText.DOFade(1,0);
        hitResultRT.DOAnchorPosY(hitResultY,.5f).OnComplete( () =>
        StartCoroutine(ResultDelay()));
    }

    IEnumerator ResultDelay()
    {
        yield return new WaitForSeconds(.33f);
        hitResultText.DOFade(0,.5f).OnComplete(
        ()=> hitResultRT.DOAnchorPosY(ogY,0));
    }
    
    public void Land(bool player)
    {   
        if(spinning)
        {
            spinning = false;
            int num = faces.FirstOrDefault(x => x.Value == picture.sprite).Key;
            number = num;
            picture.sprite = faces[number];
            DiceManager.inst.Roll(player);
        }
    }

    public IEnumerator Spin()
    {
        spinSound.pitch = Random.Range(.9f,1.1f);
        spinSound.Play();
        spinning = true;
        int i = Random.Range(0,5);
        while(spinning)
        {   
            if(i < 5)
            {i++;}
            else
            {i = 0;}
            picture.sprite = faces[i];
            yield return new WaitForSeconds(.075f);
            yield return null;
        }
        yield return null;
        spinning = false;
        spinSound.Stop();
        landSound.Play();
    }
    
    void ApplyGraphic(int i)
    {picture.sprite = faces[i];}
}