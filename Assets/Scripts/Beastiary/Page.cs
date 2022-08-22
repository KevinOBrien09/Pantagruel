using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Page : MonoBehaviour
{
    public enum PageState{Left,Right};
    public PageState pageState;
    [SerializeField] Animator anim;
    [SerializeField] Book book;

    public void TurnLeft()
    {
        

    }
}
