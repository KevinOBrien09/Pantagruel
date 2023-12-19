using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;
    
    public void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
        DialogManager.inst.StartConversation(dialog);
        Destroy(gameObject);
        }
        
    }
}