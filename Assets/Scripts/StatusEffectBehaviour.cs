using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusEffectBehaviour : MonoBehaviour
{
    public Image picture;
    public TextMeshProUGUI stackText;
    public GameObject stackGO;

    public void Init(Sprite s,StatusEffectEffect effect){
        picture.sprite = s;
        if(!effect.stackable){
            stackGO.SetActive(false);
        }
    }
    
}