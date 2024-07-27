using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
public class ZodiacPuzzleMainShrine: Interactable
{
    public List<ZodiacPuzzleSubShrine> subShrines = new List<ZodiacPuzzleSubShrine>();
    bool solved;
    public Dialog unsolved,solvedDialog;
    public GameObject result;
    

    void Start(){
        if(result != null){
    result.gameObject.SetActive(false);
        }
       
    }
    public override void Go()
    {  
        if(!solved){
   DialogManager.inst.StartConversation(unsolved);
        }
        else{
   DialogManager.inst.StartConversation(solvedDialog);
        }
     

    }
    public bool Solved()
    {
        if(solved)
        {return true;}
        
        foreach (var item in subShrines)
        {
            if(item.correctZodiac != item.currentZodiac)
            {return false;}
        }
        solved = true;
        if(result != null){
        result.gameObject.SetActive(true);
        }
        return true;
    }
}