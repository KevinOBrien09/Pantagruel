using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
public static class MiscFunctions
{

    public static List<Transform> GatherAllTransforms(Transform parent, List<Transform> transforms)
    {
        transforms.Add(parent);

        for (int i = 0; i < parent.childCount; i++)
        {
            GatherAllTransforms(parent.GetChild(i), transforms);
        }

        return transforms;
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

    public static string FirstLetterToUpperCaseOrConvertNullToEmptyString(string str)
    {return str?.First().ToString().ToUpper() + str?.Substring(1).ToLower();}


    #if UNITY_EDITOR
    [MenuItem("Func/RandomizeTrees")]
    static void RandomizeTrees()
    {
        foreach (TreeRotationRandomiser item in GameObject.FindObjectsOfType(typeof(TreeRotationRandomiser)))
        {
            item.Randomize();
        }
      
    }

    
  #endif

}