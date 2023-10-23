using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro ;
using DG.Tweening;

public class BeastBook : Singleton<BeastBook>
{   
    bool open;
    [SerializeField] TextMeshProUGUI beastName,beastPassiveName,beastPassiveDesc,beastClassifications,beastiaryID,flavour;
    Dictionary<int,BeastScriptableObject> beastDict = new Dictionary<int, BeastScriptableObject>();
    [SerializeField] Image beastImage;
    [SerializeField] GameObject parent,cover,main;
    [SerializeField] Vector2 hiddenPos;
    public RectTransform rt;
    public Button left,right;
    public SoundData closeBook,openBook;
    Vector2 centeredPos;
    bool coverOpened;
    bool inCoro;
    int index = 0;

    void Start(){
        open = false;
        parent.SetActive(false);
        centeredPos = rt.anchoredPosition;
        CloseToCover();

        foreach(var item in BeastBank.inst.beastDict){
           beastDict.Add( item.Value.beastData.bestiaryID, item.Value);
        }
        index = 1;
    }

    public void Toggle(){
        if(inCoro){
            return;
        }
        if(open){
            Close();
        }
        else{
            Open();
        }
    }

    public void OpenCover(){
        
        cover.SetActive(false);
        coverOpened = true;
        AudioManager.inst.GetSoundEffect().Play(openBook);
        LoadBeastInfo(beastDict[index]);
        main.SetActive(true);
    }

    public void CloseToCover(){
        cover.SetActive(true);
        coverOpened = false;
        main.SetActive(false);
    }

    public void Open(){
        if(index!= 1){
            OpenCover();
        }
        parent.gameObject.SetActive(true);
        rt.DOAnchorPos(hiddenPos,0);
        rt.DOAnchorPos(centeredPos,.25f);
        OverworldMovement.canMove = false;
        open = true;
    }

    public void Left()
    {
        index--;
        LoadBeastInfo(beastDict[index]);
        EventSystem.current.SetSelectedGameObject(null);
    }   

    public void Right()
    {

        index++;
        LoadBeastInfo(beastDict[index]);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void LoadBeastInfo(BeastScriptableObject bso)
    {
        BeastData bd = bso.beastData;
        if(bd.outLine != null){
   beastImage.sprite = bd.outLine;
        }
        else{
   beastImage.sprite = bd.mainSprite;
        }
     
        beastName.text = bd.beastName;
        beastiaryID.text = RomanNumerals.ToRoman(bd.bestiaryID);
        flavour.text = bd.flavourText;
        if(bd.passive != null)
        {
            beastPassiveName.text = bd.passive.passiveName;
            beastPassiveDesc.text = bd.passive.passiveDesc;
        }
        else{
             beastPassiveName.text = "Passive: Empty";
            beastPassiveDesc.text = "Not implemented.";
        }
        
        if(index == 1)
        {left.gameObject.SetActive(false);}
        else
        {left.gameObject.SetActive(true);}
        if(index== beastDict.Count)
        {right.gameObject.SetActive(false);}
        else
        {right.gameObject.SetActive(true);}
    }
    
    public void Close()
    {
        if(coverOpened){
              CloseToCover();
            AudioManager.inst.GetSoundEffect().Play(closeBook);
            StartCoroutine(q());
            IEnumerator q()
            {
                inCoro = true;
                yield return new WaitForSeconds(.5f);
                C();
            }
        }
        else{
            C();
        }
       

        void C(){
    rt.DOAnchorPos(hiddenPos,.3f) .OnComplete(()=>
            {
                parent.gameObject.SetActive(false);
                OverworldMovement.canMove = true;
                open = false;
                inCoro = false;
                    coverOpened = false;
            });
        }
      
    }

}