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
    public Image rightPic,leftPic;
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


    void ToggleButtons(bool b)
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

        listOfBlocks.Clear();
        ToggleButtons(false);
        index = -1;
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
        if(allowInput)
        {
             if(!dialogClick.isPlaying && Talking){
                dialogClick.Play();
            }
                
            if(Input.GetMouseButtonDown(0)|Input.GetKeyDown(KeyCode.Space))
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
        index = listOfBlocks.IndexOf(currentBlock);
        speaker = currentBlock.speaker;
        
        if(currentBlock.customName != string.Empty)
        { nameText.text = currentBlock.customName; }
        else
        { nameText.text = speaker.characterName; }

        nameText.color = speaker.charColour;
        if(speaker.characterIcon != null)
        {
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

        string dialog = currentBlock.dialog;
        if(currentBlock.isThought)
        {
            dialog = "<i>" + dialog + "</i>";
            typewriter.m_textUI.color = Color.gray;
        }
        else
        { typewriter.m_textUI.color = Color.white; }

    
      
        typewriter.Play(dialog,40,()=>
        {
           
            Talking = false;
        
        });
    }

    public void End()
    {
        allowInput = false;
        inDialog = false;
        rightPic.DOFade(0,.2f);
        leftPic.DOFade(0,.2f);
 
        OverworldMovement.canMove = true;
        Interactor.inst.RenableInteraction();
        typewriter.Play("",40,()=>
        {
            Talking = false;
        
        });
        
        nameText.text = "";
        StartCoroutine(q());
        IEnumerator q()
        {
            yield return new WaitForSeconds(.175f);
            ToggleButtons(true);
        }
     
    }
    
   
}
