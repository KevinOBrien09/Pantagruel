using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using KoganeUnityLib;

public class StarterSelector : Singleton<StarterSelector>
{
    public GameObject go;
    public CanvasGroup bg;
    public SoundData badum,XDXDXD;
    public StarterSelectorObject selectorPrefab;
    public RawImage rawImage;
    public TextMeshProUGUI selectX;
    public List<StarterSelectorClicker> clickers = new List<StarterSelectorClicker>();
    public TMP_Typewriter typewriter,desc;
    StarterSelectorObject selector;
    public GameObject arrows,confirm,confirmScreen;
    public Beast beastPrefab;
    public bool firstClick;
   public AudioSource song;
   public Dialog dialogToPlayAfter;
    public void Open()
    {
        StartCoroutine(XD());
        selector = Instantiate(selectorPrefab);
        selector.typewriter = this.typewriter;
        selector.desc = this.desc;
        bg.DOFade(1,.5f);
        MusicManager.inst.dungeon.DOFade(0,.25f);
        song.Play();
        Camera selectorCam = selector.cam;
        var tex = new RenderTexture (2140, 1376, 16);
        selectorCam.targetTexture = tex;
        rawImage.texture = tex;

        selector.Init(clickers);
        BattleTicker.inst.Type("Select a beast.");
        IEnumerator XD()
        {
            AudioManager.inst.GetSoundEffect().Play(badum);
            yield return new WaitForSeconds( PlayerManager.inst.movement.ZOOMPOV());
            go.SetActive(true);
            bg.DOFade(0,0);
            
       
        }
      
    }

    public void Left(){
        selector.Left();
    }

    public void Right(){
        selector.Right();
    }

    public void Confirm(){
        arrows.SetActive(false);
        selectX.text = "Select " + selector.selectedBeast.beastData.beastName + "?";
        confirm.gameObject.SetActive(false);
        confirmScreen.SetActive(true);
    }

    public void ReturnFromConfirm(){
        confirmScreen.SetActive(false);
         confirm.gameObject.SetActive(true);
        arrows.SetActive(true);
    }

    public void DefiniteConfirm()
    {
        // if(selector.selectedBeast == null){
        //     Debug.Log("XDXDXDXDXDXDXDXDXDXDXDX");
        //     selector.selectedBeast =selector.Move();
        // }


        AudioManager.inst.GetSoundEffect().Play(badum);
        bg.DOFade(0,.5f).OnComplete(()=>{
            Destroy(selector.gameObject);
            Destroy(gameObject);
            Destroy(confirm.gameObject);
        });
      
        PlayerManager.inst.movement.ResetPOVTimer();
        song.DOFade(0,.25f);
        MusicManager.inst.dungeon.DOFade(  MusicManager.inst.dungvol,.25f);
        Beast b = Instantiate(beastPrefab,PlayerParty.inst.transform);
        EXP e = new EXP(); 
        e.PsudeoLevel(5,b);
        b.Init(b.PsudeoSave(selector.selectedBeast));
        b.ownership = EntityOwnership.PLAYER;
        b.FirstHealthInit(e);
        PlayerParty.inst.AddNewBeast(b);
        BattleTicker.inst.Type(LocationManager.inst.currentSubLocation.locationName);
        if(dialogToPlayAfter == null){
            DialogManager.inst.Reset();
        }
        else{
            DialogManager.inst.StartConversation(dialogToPlayAfter);
        }
    }

    
}