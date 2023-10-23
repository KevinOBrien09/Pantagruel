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
    public int index = -1;
    [SerializeField] Button[] buttonsToDisable;
    [SerializeField] TMP_Typewriter[] typewriters;
    [SerializeField] TextMeshProUGUI nameText;
    List<DialogBlock> listOfBlocks = new List<DialogBlock>();
    Queue<DialogBlock> blocks = new Queue<DialogBlock>();
    Queue<DialogBlock> previousBlocks = new Queue<DialogBlock>();
    bool allowInput;
    bool talking;

    void Start(){
        nameText.text = "";
         typewriters[0].Play("",40,()=>
        {
            Talking = false;
        
        });
 
    }

    [HideInInspector] public bool Talking
    {
        get { return talking ; }
        set
        {
            if( value == talking )
            { return; }
            // talking = value ;
            // if(talking)
            // { 
            //     if(displayedActors.Contains(speaker))
            //     { speaker.uiActor.mouth.Play(.1f,true); }
            
            // }
            // else
            // { 
            //     if(displayedActors.Contains(speaker))
            //     { speaker.uiActor.mouth.Stop(); }
            // }   
        } 
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
            
                
            if(Input.GetMouseButtonDown(0)|Input.GetKeyDown(KeyCode.Space))
            { 
                if(Talking)
                {   
                    typewriters[0].Skip();
                    typewriters[1].Skip();
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
        index = listOfBlocks.IndexOf(currentBlock);
        speaker = currentBlock.speaker;
        
        if(currentBlock.customName != string.Empty)
        { nameText.text = currentBlock.customName; }
        else
        { nameText.text = speaker.characterName; }

        nameText.color = speaker.charColour;

        string dialog = currentBlock.dialog;
        if(currentBlock.isThought)
        {
            dialog = "<i>" + dialog + "</i>";
            typewriters[0].m_textUI.color = Color.gray;
        }
        else
        { typewriters[0].m_textUI.color = Color.white; }

    
        Talking = true;
        typewriters[0].Play(dialog,40,()=>
        {
            Talking = false;
        
        });
    }

    public void End()
    {
        allowInput = false;
        inDialog = false;
       
        OverworldMovement.canMove = true;
        Interactor.inst.RenableInteraction();
        typewriters[0].Play("",40,()=>
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
