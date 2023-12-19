using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable: MonoBehaviour
{
    

    public virtual void Go()
    {
       
        Debug.Log(gameObject.name);
    }


}