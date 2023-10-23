using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor: Singleton<Interactor>
{
    public float rayDist;
    public LayerMask mask;
    public bool canInteract;
    void Update(){

        if(canInteract)
        {
            if(InputManager.input.space)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position,transform.forward *rayDist,maxDistance: rayDist,hitInfo: out hit,layerMask:mask))
                {
                    DialogHolder h = null;
                    bool poop = hit.transform.gameObject. TryGetComponent<DialogHolder>(out h);
                    if(poop){
                        DialogManager.inst.StartConversation(h.dialog);
                        canInteract = false;
                    }
                    Debug.Log(hit.transform.gameObject.name + poop);
                }
                
            }
        }
            
    }

    public void RenableInteraction()
    {
        StartCoroutine(q());
        IEnumerator q(){
            yield return new WaitForSeconds(.15f);
            canInteract = true;
        }

    }

}