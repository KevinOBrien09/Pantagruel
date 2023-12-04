 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NESW : Singleton<NESW>
{
    public GameObject test;
    public CardinalDirection GetDirection(Transform yRot)
    {
        float a = UnwrapAngle(yRot.localEulerAngles.y);
        if (a ==0 ||  a >= 360) 
        {return CardinalDirection.N;}
        else  if (a <= 90 && a > 0) 
        {return CardinalDirection.E;}
        else  if (a <= 180 && a > 90) 
        {return CardinalDirection.S;}
        else  if  (a <= 270 && a > 180) 
        {return CardinalDirection.W;}
        Debug.LogAssertion("DEFAULT CASE");
        return CardinalDirection.N;;
    }

    float UnwrapAngle(float angle)
        {
            if(angle >=0)
                return angle;
 
            angle = -angle%360;
 
            return 360-angle;
        }


}