using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CardinalDirectionDisplay : Singleton<CardinalDirectionDisplay>
{
    [System.Serializable]
    public class CardDirX
    {
        public float x;
        public CardinalDirection cardinalDirection;


    }
    public RectTransform holder;
    public List<TextMeshProUGUI> dirText = new List<TextMeshProUGUI>();
    public float rightMove,leftMove;
    protected override void Awake()
    {
        base.Awake();
        
    }

    public void InitDisplay(CardinalDirection cardinalDirection)
    {
        dirText[1].text = q(cardinalDirection);
      
    }

    public void ChangeDisplay(CardinalDirection newRot,CardinalDirection previousRot)
    {
        bool northToWest = previousRot == CardinalDirection.N && newRot == CardinalDirection.W;
        bool westToSouth = previousRot == CardinalDirection.W && newRot == CardinalDirection.S;
        bool southToEast = previousRot == CardinalDirection.S && newRot == CardinalDirection.E;
        bool eastToNorth = previousRot == CardinalDirection.E && newRot == CardinalDirection.N;

        bool northToEast = previousRot == CardinalDirection.N && newRot == CardinalDirection.E;
        bool eastToSouth = previousRot == CardinalDirection.E && newRot == CardinalDirection.S;
        bool southToWest = previousRot == CardinalDirection.S && newRot == CardinalDirection.W;
        bool westToNorth = previousRot == CardinalDirection.W && newRot == CardinalDirection.N;

        bool left = northToWest | westToSouth | southToEast | eastToNorth;
        bool right = northToEast| eastToSouth | southToWest | westToNorth;

       
       

        int prev = 0;
        int otherOne = 0;

        if(right)
        {
            holder.anchoredPosition = new Vector2(leftMove,0);
            prev = 0;
            otherOne = 2;
        }
        else
        {   
            holder.anchoredPosition = new Vector2(rightMove,0);
            prev = 2;
            otherOne = 0;
        }
        holder.DOAnchorPosX(0,.25f);

        dirText[1].text = q(newRot);
        dirText[prev].text = q(previousRot);
        if(northToEast | northToWest)
        {dirText[otherOne].text = q(CardinalDirection.S);}
        else if(eastToSouth | westToSouth)
        {dirText[otherOne].text = q(CardinalDirection.W);}
        else if(southToWest | southToEast)
        {dirText[otherOne].text = q(CardinalDirection.N);}
        else if(southToWest | eastToNorth) 
        {dirText[otherOne].text = q(CardinalDirection.E);}
      
    }
  string q(CardinalDirection c)
    {
        switch(c)
        {
            case CardinalDirection.N:
            return "North";
            case CardinalDirection.E:
            return "East";
            case CardinalDirection.S:
            return "South";
            case CardinalDirection.W:
            return "West";
        }

        return "";
    }
}