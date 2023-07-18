using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaGem : MonoBehaviour
{
    public Image BG;
    public Image main;
    public Image border;
    public bool active;

    public void Activate(){
        active = true;
       TurnOnCircle();
        gameObject.SetActive(true);
    }

    public void TurnOffCircle(){
        main.gameObject.SetActive(false);
    }

    public void TurnOnCircle(){
        main.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        active = false;
       
        gameObject.SetActive(false);
    }

}