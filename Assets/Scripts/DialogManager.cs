using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;
using KoganeUnityLib;


public class DialogManager : Singleton<DialogManager>
{
    public Dialog currentConversation;
    public Character speaker;
    public bool inDialog;
    public Image rightPic,leftPic,comic;
    public int index = -1;
    [SerializeField] Button[] buttonsToDisable;
    [SerializeField] TMP_Typewriter typewriter;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] AudioSource dialogClick;
    List<DialogBlock> listOfBlocks = new List<DialogBlock>();
    Queue<DialogBlock> blocks = new Queue<DialogBlock>();
    Queue<DialogBlock> previousBlocks = new Queue<DialogBlock>();
    bool allowInput;
    bool Talking;
    bool musicWasChanged;
    public bool freeze;
    public Vector2 leftSilent,leftSpeaking,rightSilent,rightSpeaking;
    void Start(){
        nameText.text = "";
         typewriter.Play("",40,()=>
        {
            Talking = false;
        
        });
        rightPic.DOFade(0,0);
        leftPic.DOFade(0,0);
 
    }


   public void ToggleButtons(bool b)
    {
        foreach (var item in buttonsToDisable)
        {item.interactable = b;}
    }
    
    public void StartConversation(Dialog newConversation)
    {
        // if(conversationState == ConversationType.DEBATE)
        // {
        //     if(newConversation.conversationType == ConversationType.NORMAL && !newConversation.keepReputationBarOnScreen)
        //     {ReputationManager.inst.Hide();}
        // }
        comic.DOFade(0,0);
        listOfBlocks.Clear();
        ToggleButtons(false);
        index = -1;
         Interactor.inst. interactText.SetActive(false);
       // conversationState = newConversation.conversationType;

        // if(conversationState == ConversationType.DEBATE)
        // {
        //     ReputationManager.inst.Show();
        // }
      
        OverworldMovement.canMove = false;
        // RotatePlayer.inst.canLook = false;
        // Player.inst.allowSceneInput = false;
        blocks.Clear();
        foreach (var item in newConversation.blocks)
        {
            blocks.Enqueue(item);
            listOfBlocks.Add(item);
        }
        allowInput = true;
        currentConversation = newConversation;
        inDialog = true;
        if(blocks.Count!=0)
        {
            DialogBlock currentBlock =  blocks.Dequeue();
            ProcessBlock(currentBlock);
        }
    }

    void Update()
    {
        if(freeze){
            return;
        }
        if(allowInput)
        {
             if(!dialogClick.isPlaying && Talking){
                dialogClick.Play();
            }
                
            if(Input.GetMouseButtonDown(0)|Input.GetKeyDown(KeyCode.Space) | Input.GetKey(KeyCode.PageUp)) //CHANGE
            { 
                if(Talking)
                {   
                    typewriter.Skip();
                   
                }
                else
                {
                    if(blocks.Count!=0)
                    {
                        DialogBlock currentBlock =  blocks.Dequeue();
                        ProcessBlock(currentBlock);
                    }
                    else
                    {End();}
                }
                    
                
            }
        }
    }

    public void ChangeBlock(int i)
    {
        if(listOfBlocks.Count!=0)
        {
            if(listOfBlocks.ElementAtOrDefault(i) != null)
            {
                DialogBlock currentBlock =  listOfBlocks[i];
                ProcessBlock(currentBlock);
            }
        }
    }

    public void ProcessBlock(DialogBlock currentBlock)
    {
        Talking = true;
        CameraManager.inst.ChangeCameraState(currentBlock.cameraState);
        AudioManager.inst.GetSoundEffect().Play(currentBlock.soundEffect);
        index = listOfBlocks.IndexOf(currentBlock);
        speaker = currentBlock.speaker;

        if(currentBlock.comic != null){
            comic.gameObject.SetActive(true);
            comic.sprite = currentBlock.comic;
            if(comic.color.a != 1){
                
                comic.DOFade(1,.25F);
            }
           
        }
        else{
            if(comic.gameObject.activeSelf){
                comic.DOFade(1,.25F).OnComplete(()=>{
                comic.gameObject.SetActive(false);
                });
                 
            }
        }

        if(currentBlock.worldEvents.Length != 0){


            foreach (var item in currentBlock.worldEvents)
            {
                if(BattleManager.inst.inTutorial){
                    if(item != string.Empty)
                    {
                        if(WorldEventManager.inst != null)
                        {
                          TutorialManager.inst.ProcessEvent(item);
                        }
                        else
                        {Debug.LogAssertion("No WorldEvent Manager");}
                    
                    }
                }
                else{
 if(item != string.Empty)
                {
                    if(WorldEventManager.inst != null)
                    {
                        WorldEventManager.inst.ProcessEvent(item);
                    }
                    else
                    {Debug.LogAssertion("No WorldEvent Manager");}
                
                }
                }
               
            }
        }
      
        if(currentBlock.move){
  if(currentBlock.moveDir >= 0){
        PlayerManager.inst.movement.StartMove(currentBlock.moveDir);
        }
        }
      

        if(currentBlock.rotation.forceRotation)
        {  PlayerManager.inst.movement.rotate.ForceRotation(currentBlock.rotation.playerYRotation);

        }

        if(currentBlock.changeMusic.audioClip != null){
            MusicManager.inst.ChangeMusic(currentBlock.changeMusic);
            musicWasChanged = true;
        }
        
        if(currentBlock.customName != string.Empty)
        { nameText.text = currentBlock.customName; }
        else
        { nameText.text = speaker.characterName; }

        nameText.color = speaker.charColour;
        if(speaker.characterIcon != null)
        {
            if(currentBlock.isThought)
            {
                rightPic.DOFade(0,.2f);
                leftPic.DOFade(0,.2f);
            }
            else{
                if(!currentBlock.showLeft)
                {
                    rightPic.DOFade(1,.25f);
                    rightPic.sprite = speaker.characterIcon;
                    rightPic.GetComponent<RectTransform>().DOAnchorPos(rightSpeaking,.2f);
                    leftPic.GetComponent<RectTransform>().DOAnchorPos(leftSilent,.2f);
                }
                else
                {
                    leftPic.DOFade(1,.25f);
                    leftPic.sprite = speaker.characterIcon;
                    rightPic.GetComponent<RectTransform>().DOAnchorPos(rightSilent,.2f);
                    leftPic.GetComponent<RectTransform>().DOAnchorPos(leftSpeaking,.2f);
                }
            }
           
           
        }
      
        string dialog = currentBlock.dialog;
        if(currentBlock.isThought)
        {
            dialog = "<i>" + dialog + "</i>";
            typewriter.m_textUI.color = Color.gray;
        }
        else
        { typewriter.m_textUI.color = Color.white; }

    
      
        typewriter.Play(Parse(dialog),40,()=>
        {
           
            Talking = false;
            if(currentBlock.end){
            End();
        }
        
        });

        
    }

    public string Parse(string s){
        if(s.Contains("@NEWCARD@"))
        {
            s = s.Replace("@NEWCARD@",CardCollection.inst.cardPickUp.cardName);
            
        }

         if(s.Contains("@NEWITEM@"))
        {
            s = s.Replace("@NEWITEM@",Inventory.inst.itemPickedUp.itemName);
            
        }
        return s;
    }

    public void End()
    {
        if(BattleManager.inst.inTutorial){
            TutorialManager.inst.Leave();
            return;
        }
CardCollection.inst.cardPickUp = null;
        if(currentConversation.locationDialog.moveAfterDialog)
        {
            if(currentConversation.locationDialog.subLoc != null)
            {
                 LocationManager.inst.ChangeSubLocationWithFade
                (currentConversation.locationDialog.subLoc,currentConversation.locationDialog.subLocRot,currentConversation.locationDialog.subLocPos);
                if(currentConversation.locationDialog.subLoc.dialogToLoadOnEnter != null){
                    return;
                }



               
            }
            else if( currentConversation.locationDialog.mainLocation != null)
            {
                LocationManager.inst.ChangeMainLocationWithFade(currentConversation.locationDialog.mainLocation);
               
                if(currentConversation.locationDialog.mainLocation.subLocations[0].dialogToLoadOnEnter != null){
                          OverworldMovement.canMove = false;
                    return;
                }



            }
            else{
                 PlayerManager.inst.movement.InitPosRot(currentConversation.locationDialog.subLocPos,currentConversation.locationDialog.subLocRot);
                  PlayerManager.inst.movement.rotate.InitRotation(NESW.inst.GetDirection(PlayerManager.inst.movement.rotate.transform));
            }
           
            CameraManager.inst.ChangeCameraState(CameraState.NORMAL);
            allowInput = false;
            inDialog = false;
           HidePotraits();
            typewriter.Play("",40,()=>
            {Talking = false;});
        
            nameText.text = "";
            ToggleButtons(true);
            //Interactor.inst.RenableInteraction();
            return;
        }
        if(musicWasChanged && currentConversation.changeMusicBack){
            MusicManager.inst.ChangeMusic(LocationManager.inst.currentSubLocation.overworldMusic);
            musicWasChanged = false;
        }

        if(comic.gameObject.activeSelf){
            comic.DOFade(1,.25F).OnComplete(()=>{
            comic.gameObject.SetActive(false);
            });
                 
        }

      
        if(!currentConversation.doNotReset){
           Reset();
        }
        
        if(currentConversation.moveDir >= 0){
        PlayerManager.inst.movement.StartMove((Dir)currentConversation.moveDir);
        }
       
     
     
        CameraManager.inst.ChangeCameraState(CameraState.NORMAL);
       
     
    }

    public void HidePotraits(){
 rightPic.DOFade(0,.2f);
            leftPic.DOFade(0,.2f);
    }

    public void ClearNameAndDialogText(){
        
            nameText.text = "";
          typewriter.Play("",40,()=>
            {
                Talking = false;
            
            });
    }

    public void Reset(){
 allowInput = false;
            inDialog = false;
            rightPic.DOFade(0,.2f);
            leftPic.DOFade(0,.2f);
            OverworldMovement.canMove = true;
          ClearNameAndDialogText();
        
           Interactor.inst.RenableInteraction();
           BattleManager.inst. StartCoroutine(q());;
            IEnumerator q()
            {
                yield return new WaitForSeconds(.175f);
                  Interactor.inst.CheckForInteraction();
                ToggleButtons(true);
            }
    }
    
   
}
