 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class BattleIntro:Singleton<BattleIntro>                   
{
    public RectTransform enemySlider,playerSlider;
    public Vector2 enemyHiddenPos,playerHiddenPos,eafter,pafetr;
    Vector2 playerSliderShownPos,enemySliderShownPos;
    public Image playerBeastPic,enemyBeastPic,enemyBeastFill;
    public TextMeshProUGUI enemyBeastName,playerBeastName;
    public AudioSource click1,click2;
    public CanvasGroup group;
    void Start()
    {
        enemySliderShownPos = enemySlider.anchoredPosition;
        playerSliderShownPos = playerSlider.anchoredPosition;
        enemySlider.anchoredPosition = enemyHiddenPos;
        playerSlider.anchoredPosition = playerHiddenPos;
        gameObject.SetActive(false);
        group.alpha = 0;
    }

    public void EnterBattle(Beast playerBeast,Beast enemyBeast)
    {
        BeastData e =  enemyBeast.scriptableObject.beastData;
        BeastData p =  playerBeast.scriptableObject.beastData;
//MusicManager.inst.EnterBattle();
        gameObject.SetActive(true); 
        group.DOFade(1,.1F);
        BattleTicker.inst.Type("Fight or flight");
        click1.Play();
        playerBeastPic.sprite =p.mainSprite;
        playerBeastName.text = p.beastName;
        enemyBeastPic.sprite =e.mainSprite;
        enemyBeastName.text = e.beastName;
        enemySlider.DOAnchorPos(enemySliderShownPos,.2f);
        playerSlider.DOAnchorPos(playerSliderShownPos,.2f);

        StartCoroutine(q());
        IEnumerator q()
        {

            yield   return new WaitForSeconds(1f);
            enemySlider.DOAnchorPos(eafter,.2f);
            playerSlider.DOAnchorPos(pafetr,.2f);
            yield   return new WaitForSeconds(.2f);
               group.DOFade(0,.3F);
              click2.Play();
            BattleTicker.inst.Type("Turn " +  BattleManager.inst.turn.ToString());
            BattleManager.inst.EnterBattlePartTwo();
        }

    }


}