using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class EntityIcon : MonoBehaviour,IPointerClickHandler
{
    public Image picture;
    public HealthBar healthBar;
    public RectTransform rt;
    public Image selected;
    Entity e;

    public void InitBeast(Beast b)
    {
        healthBar.entity = b;
        b.currentHealthBars.Add(healthBar);
        healthBar.onInit.Invoke();
        picture.sprite = b.scriptableObject.beastData.uiPicture;
        selected.DOFade(0,0);
        e = b;
    }

    public void InitPet(PetBehaviour p)
    {
        healthBar.entity = p;
        p.currentHealthBars.Add(healthBar);
        healthBar.onInit.Invoke();
        picture.sprite = p.pet.uiPicture;
        e = p;
    }

    public void Move(float y)
    {rt.DOAnchorPosY(y,.4f);}

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button ==PointerEventData.InputButton.Right && BattleManager.inst.inBattle){
            var beast = e as Beast;
            var pet = e as PetBehaviour;

            if (beast != null)
            {
                BeastProfileViewer.inst.Open(beast);
                CardManager.inst.DeactivateHand();
            
            }
            else if(pet != null)
            {
            
            }
        }
        
    }

  

    
}