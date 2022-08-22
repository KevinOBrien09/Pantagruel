using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Image healthBarPicture;
    [SerializeField] Beast currentBeast;

    public void Apply(Beast b)
    {
        if(b.allience == Alliance.Player)
        {healthText.text = b.currentHealth.ToString();}
        else
        {healthText.text = b.data.beastName + " : " + b.currentHealth.ToString();}

        healthBarPicture.DOFillAmount((float)b.currentHealth/(float)b.data.stats.maxHealth,.2f);
    
        
        if(currentBeast != null)
        {currentBeast.onHit -= DelayChange;
        currentBeast.onHeal -= DelayChange;}

        currentBeast = b;
        currentBeast.healthBar = this;
        currentBeast.onHit += DelayChange;
        currentBeast.onHeal += DelayChange;
    }  

    public void DelayChange(int d){
        StartCoroutine(Change(d));
    }

    public IEnumerator Change(int dmg)
    {
        yield return new WaitForSeconds(.3f);
        UpdateHealthBar();
    } 

    public void UpdateHealthBar(){
        healthBarPicture.DOFillAmount((float) currentBeast.currentHealth/(float) currentBeast.data.stats.maxHealth,.35f);
        if(currentBeast.allience == Alliance.Player)
        {healthText.text = currentBeast.currentHealth.ToString();}
        else
        {healthText.text = currentBeast.data.beastName + " : " + currentBeast.currentHealth.ToString();}
    }
   
}
