using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDie : MonoBehaviour
{
    public bool full;
    public Image image;

    public void DeActivate()
    {
        full = false;
        image.sprite = null;
        gameObject.SetActive(false);
    }

    public void Activate(Sprite s)
    {
        full = true;
        image.sprite = s;
        gameObject.SetActive(true);
    }

}