using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnStart: MonoBehaviour
{
    void Awake(){
        Destroy(gameObject);
    }


}