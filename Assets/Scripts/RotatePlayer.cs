using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public enum Dir{Forwards,Backwards,Left,Right}
public enum CardinalDirection{N,E,S,W}
public class RotatePlayer : MonoBehaviour
{
 
   
    public CardinalDirection currentRot;
    [SerializeField] float rotateSpeed;
    [SerializeField] OverworldMovement move;
    [SerializeField] TextMeshProUGUI dirText;

    public bool isRotating;

   
    public void StartRotate(Dir dir)
    {
        if(OverworldMovement.canMove)
        {
            if(!isRotating & !move.isMoving)
            {
                if(dir.Equals(Dir.Left))
                {Rotate(-90f);}
                else
                {Rotate(90f);}

            }
        }
        
    }

    public void Rotate(float num)
    {
        
        isRotating = true;
        float newRot = transform.rotation.eulerAngles.y+num;
        CardinalDirection previousRot = currentRot;
        transform.DORotate(new Vector3(transform.rotation.eulerAngles.x,newRot,transform.rotation.eulerAngles.z),rotateSpeed).OnComplete(() => isRotating = false);
        if(num < 0)
        {
            if(currentRot == CardinalDirection.N)
            {currentRot = CardinalDirection.W;}
            else if(currentRot == CardinalDirection.W)
            {currentRot = CardinalDirection.S;}
            else if(currentRot == CardinalDirection.S)
            {currentRot = CardinalDirection.E;}
            else if(currentRot == CardinalDirection.E)
            {currentRot = CardinalDirection.N;}
        }
        else
        {
            if(currentRot == CardinalDirection.N)
            {currentRot = CardinalDirection.E;}
            else if(currentRot == CardinalDirection.E)
            {currentRot = CardinalDirection.S;}
            else if(currentRot == CardinalDirection.S)
            {currentRot = CardinalDirection.W;}
            else if(currentRot == CardinalDirection.W)
            {currentRot = CardinalDirection.N;}
        }

        if(dirText != null)
        {dirText.text = currentRot.ToString();}

        if(CardinalDirectionDisplay.inst != null)
        {CardinalDirectionDisplay.inst.ChangeDisplay(currentRot,previousRot);}
    }
}