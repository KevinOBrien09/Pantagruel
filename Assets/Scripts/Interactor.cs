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
                    if(!PlayerManager.inst.movement.isMoving && !PlayerManager.inst.movement.rotate.isRotating){
Interactable i = null;
                    bool dia = hit.transform.gameObject. TryGetComponent<Interactable>(out i);
                    if(dia){
                      i.Go();  
                      canInteract = false;
                    }
                   
                    }
                   
                  
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