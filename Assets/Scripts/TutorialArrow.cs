using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class TutorialArrow : MonoBehaviour
{

    public RectTransform rt;
    public CardinalDirection dir;
    public GenericDictionary<CardinalDirection,Vector3> rots = new GenericDictionary<CardinalDirection, Vector3>();
    public float intestity,speed;
    Vector2 startPos;
    Vector2 endPos;
    void Start(){
        rt = GetComponent<RectTransform>();
        startPos = rt.anchoredPosition;
        Vector3 r = rots[dir];
        rt.rotation = Quaternion.Euler(r.x,r.y,r.z);
        switch(dir)
        {
            case CardinalDirection.N:
            endPos = new Vector2(startPos.x,startPos.y+intestity);
            break;
            case CardinalDirection.E:
            endPos = new Vector2(startPos.x-intestity,startPos.y);
            break;
            case CardinalDirection.S:
            endPos = new Vector2(startPos.x,startPos.y-intestity);
            break;
            case CardinalDirection.W:
            endPos = new Vector2(startPos.x+intestity,startPos.y);
            break;
        }

        Bounce();
   }

   void Bounce()
   {
        rt.DOAnchorPos(endPos,speed).OnComplete(()=>{

            rt.DOAnchorPos(startPos,speed).OnComplete(()=>{
                Bounce();
            });
        });
   }

    
}

    


