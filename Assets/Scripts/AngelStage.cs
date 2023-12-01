using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelStage:Singleton<AngelStage>                   
{
    public GameObject GO;
    public void Toggle(){
        if(GO.activeSelf){
            GO.SetActive(false);
        }
        else{
            GO.SetActive(true);
        }
    }
}