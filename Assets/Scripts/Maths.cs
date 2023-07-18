using System;
using UnityEngine;

public static class Maths
{
    public static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        var boundary = (int)Math.Floor(Math.Sqrt(number));
            
        for (int i = 3; i <= boundary; i += 2)
            if (number % i == 0)
                return false;
        
        return true;        
    }

    public static bool FiftyFifty()
    {
        if(UnityEngine.Random.Range(0,2) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public static double Percent(this double number,int percent)
    {
      
        return ((double)number * percent) / 100;
    }

    public static bool PercentCalculator(int percent)
    {
        if(percent >= UnityEngine.Random.Range(0,101))
        {
            return true;
        }
        else return false;

    }
     
    

}