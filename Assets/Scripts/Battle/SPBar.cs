using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class SPBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI spText;
    [SerializeField] Image spBarPicture;
    [SerializeField] Beast currentBeast;

    public void Apply(Beast b)
    {
        spBarPicture.DOFillAmount((float)b.currentSP/(float)b.data.resource.amount,.2f);
        spText.text = b.currentSP.ToString();
        
        if(currentBeast != null)
        {currentBeast.on_SP_Drain -= Change;}

        currentBeast = b;
        currentBeast.on_SP_Drain += Change;
    }  

    public void Change(int dmg)
    {
        spBarPicture.DOFillAmount((float)currentBeast.currentSP/(float)currentBeast.data.resource.amount,.2f);
        spText.text =currentBeast.currentSP.ToString();
    } 
   
}
