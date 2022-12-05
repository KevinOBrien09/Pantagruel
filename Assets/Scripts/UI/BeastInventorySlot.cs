using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BeastInventorySlot : MonoBehaviour
{
    public Beast beast;
    public Image image;
    public TextMeshProUGUI hp,sp,name;
    public GameObject empty;
    public Button swapButton;

    void Start(){
        empty.SetActive(true);
         hp.gameObject.SetActive(false);
        sp.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
           name.gameObject.SetActive(false);
    }

    
    public void ApplyBeast(Beast b)
    {    
        if(b!=null){
 if( 0 >= b.currentHealth)
        {
            swapButton.interactable = false;
            empty.GetComponent<TextMeshProUGUI>().text = "DEAD.";
        }

        empty.SetActive(false);
        hp.gameObject.SetActive(true);
        sp.gameObject.SetActive(true);
        image.gameObject.SetActive(true);
        name.gameObject.SetActive(true);
        name.text = b.data.beastName;

        beast = b;
        image.sprite = b.data.uiPicture;
        hp.text = "HP :" + b.currentHealth.ToString();
        sp.text = "SP :" + b.currentSP.ToString();
        }else{
              empty.SetActive(true);
         hp.gameObject.SetActive(false);
        sp.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
           name.gameObject.SetActive(false);

        }
       
    }

    public void Swap()
    {
        if(beast != null){
 if(BattleManager.inst.activePlayerBeast!=beast){
  UIManager.inst.DeactivateSwapBeastButton();
        UIManager.inst.Normal();
        PlayerInventory.inst.ChangeActiveBeast(beast);
        
        if( BattleManager.inst.callBM){
            BattleManager.inst.callBM = false;
             MainTextTicker.inst.Type(beast.data.beastName+".");
            BattleManager.inst.StartCoroutine( BattleManager.inst.EndOfTurnDelay());
        }
        }
        }
       
      
    }
}
