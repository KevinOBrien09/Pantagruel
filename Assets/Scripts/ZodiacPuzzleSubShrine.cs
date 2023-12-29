using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
public class ZodiacPuzzleSubShrine: Interactable
{
    public GenericDictionary<Zodiac,float> dict = new GenericDictionary<Zodiac,float>();
    public Transform cube;
    public Zodiac startingZodiac,correctZodiac,currentZodiac;
    public ZodiacPuzzleMainShrine mainShrine;
    public Dialog solved1,solved2;
    public int index;
    bool rotating;
   
  
    void Start()
    {
        index = dict.Keys.ToList().IndexOf(startingZodiac);
        Rotate();
    }

    public override void Go()
    { 
  
        if( rotating|| mainShrine.Solved())
        {return;}
        Interactor.inst.RenableInteraction();
        rotating = true;
        index++;
        if(index >= dict.Count)
        {index = 0;}
        Rotate();
       
    }
    
    public void Rotate()
    {
        currentZodiac = dict.ElementAt(index).Key;
        Vector3 v  = new Vector3(dict.ElementAt(index).Value,   cube.transform.localEulerAngles.y,   cube.transform.localEulerAngles.z);
        cube.DOLocalRotate(v,.25f).OnComplete(()=>{rotating = false;});
        if(mainShrine != null){
            if(mainShrine.Solved()){
                DialogManager.inst.StartConversation(solved1);
                
            }
        }

    }


}