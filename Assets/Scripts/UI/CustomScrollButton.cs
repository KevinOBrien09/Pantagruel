using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class CustomScrollButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public CustomScroll scroll;
    public bool up;
    bool hold;
    [SerializeField] float speed;
    public void OnPointerDown(PointerEventData eventData)
    {
        hold = true;
        StartCoroutine(Thing());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        hold = false;
    }

    IEnumerator Thing()
    {
        while(hold)
        {
            if(up)
            {scroll.LerpUp(speed);}
            else
            {scroll.LerpDown(speed);}
            yield return null;
        }
    }
}
