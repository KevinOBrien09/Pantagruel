 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBeastHealthbar : HealthBar
{
    //public HealthBar targetedPetHealthBar;
    [SerializeField] Color32 ogRed,dampenRed,grey;

    // public void SummonPetHealthBar(PetBehaviour e){
    //     targetedPetHealthBar.gameObject.SetActive(true);
    //     targetedPetHealthBar.entity = e;
    
    //     e.currentHealthBars.Add( targetedPetHealthBar);
    //     targetedPetHealthBar.onInit.Invoke();
    //     DampenHealthBar();
    // }

    // public void RemovePetHealthBar(){
    //     targetedPetHealthBar.gameObject.SetActive(false);
    //     targetedPetHealthBar.entity = null;
     
    //     UnDampenHealthBar();
    // }

    public void DampenHealthBar(){
        fill.DOColor( dampenRed,.2f);
        // current.DOColor(grey,.2f);
        // max.DOColor(grey,.2f);
    }

    public void UnDampenHealthBar(){
         fill.DOColor( ogRed,.2f);
        // current.DOColor(Color.white,.2f);
        // max.DOColor(Color.white,.2f);
    }

    
}