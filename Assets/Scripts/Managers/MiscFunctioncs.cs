using System;
using UnityEngine;

public static class MiscFunctions
{
    public static string GetAbbStatName(StatName sn)
    {
        switch (sn)
        {
            case StatName.Physical:
            return "PHYS";
            case StatName.Magic:
            return "MGK";
            case StatName.Toughness:
            return "TGH";
            case StatName.Charisma:
            return "CHR";
            case StatName.Resolve:
            return "RES";
            default:
            Debug.LogAssertion("DefaultCase");
            return "BUG";
        }
    }
    
    public static (int,string) GetPercentFromString(string s,string tag)
    {
       
        string percentString = MiscFunctions.GetStringBetweenCharacters(s,tag, '}'); 
        string totalString = tag + "{" + percentString + "}";
        int percent = int.Parse(percentString);
        return (percent,totalString);
    }
    
    public static string GetStringBetweenCharacters(string input, string tag,char charTo)
    {
        int startIndex = input.IndexOf(tag) + tag.Length + 1;//1 IS {
        char[] c = input.ToCharArray();
        string number = "";

        for (int i = startIndex; i < c.Length; i++)
        {
            if(c[i] != charTo)
            {
                number = number+c[i];
            }
            else
            {
                break;
            }
            
        }
        return number;
    }

}