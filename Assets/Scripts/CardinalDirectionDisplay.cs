using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardinalDirectionDisplay : Singleton<CardinalDirectionDisplay>
{
    [System.Serializable]
    public class CardDirX
    {
        public float x;
        public CardinalDirection cardinalDirection;


    }
    public RectTransform holder;
    public List<CardDirX> xValues = new List<CardDirX>();
    public Dictionary<CardinalDirection,float> dict = new Dictionary<CardinalDirection, float>();

    public void Start()
    {
        foreach (var item in xValues)
        {dict.Add(item.cardinalDirection,item.x);}
    }

    public void InitDisplay(CardinalDirection cardinalDirection){
          holder.DOAnchorPosX(dict[cardinalDirection],0);
    }

    public void ChangeDisplay(CardinalDirection newRot,CardinalDirection previousRot)
    {
          holder.DOAnchorPosX(dict[newRot],.2f);


        // if(previousRot == CardinalDirection.W && newRot == CardinalDirection.S)
        // {
        //     holder.DOAnchorPosX(-450,0).OnComplete(()=>{holder.DOAnchorPosX(dict[newRot],.2f);});
        // }
        // else if(previousRot == CardinalDirection.S && newRot == CardinalDirection.W){
        //     holder.DOAnchorPosX(275,0).OnComplete(()=>{holder.DOAnchorPosX(dict[newRot],.2f);});
        // }
        // else{
          
        // }
        

    }

}