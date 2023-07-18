using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomPanel:Singleton<BottomPanel>
{
    public GameObject dialog;
    public GameObject cards;

    public void ChangeState(GameObject g){
        dialog.SetActive(false);
        cards.SetActive(false);

        g.SetActive(true);
    }

}