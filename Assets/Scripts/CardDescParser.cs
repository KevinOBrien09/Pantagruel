 
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public static class CardDescParser 
{

    public static bool cardIsSwitchable(string str){
if(str.Contains("%")){
    return true;
}else{return false;}
    }
    public static string GetBeastValues(Card c,Beast b)
    {
        string s = c.desc;
        
        if(s.Contains("{PHYS"))
        {
            string poo =  Between(s,"{","}");
            string foo = poo;
            foo = foo.Replace("{",string.Empty);
            foo = foo.Replace("}",string.Empty);
            foo = foo.Replace("PHYS",string.Empty);
            foo = foo.Replace("%",string.Empty);
            int percent = int.Parse(foo);
            int amount = (int)Maths.Percent(b.stats().physical,percent);
            s = s.Replace(poo,"<color=orange>" + amount.ToString() + "</color>");
        }

        if(s.Contains("{MGK"))
        {
            string poo =  Between(s,"{","}");
            string foo = poo;
            foo = foo.Replace("{",string.Empty);
            foo = foo.Replace("}",string.Empty);
            foo = foo.Replace("MGK",string.Empty);
            foo = foo.Replace("%",string.Empty);
            int percent = int.Parse(foo);
            int amount = (int)Maths.Percent(b.stats().magic,percent);
            s = s.Replace(poo,"<color=#8C2096>" + amount.ToString() + "</color>");
        }

       return FirstLetterToUpper(ColourParse(s));
    }

  

    public static string GetPercentValues(Card c)
    {
        string s = c.desc;
        
        if(s.Contains("{PHYS"))
        {
           
            string poo =  Between(s,"{","}");
            string foo = poo;
            foo = foo.Replace("{",string.Empty);
            foo = foo.Replace("}",string.Empty);
            foo = foo.Replace("PHYS",string.Empty);
            foo = foo.Replace("%",string.Empty);
            int percent = int.Parse(foo);
            
            s = s.Replace(poo,percent+ "% <b>phys</b>");
        }

        if(s.Contains("{MGK"))
        {
           
            string poo =  Between(s,"{","}");
            string foo = poo;
            foo = foo.Replace("{",string.Empty);
            foo = foo.Replace("}",string.Empty);
            foo = foo.Replace("MGK",string.Empty);
            foo = foo.Replace("%",string.Empty);
            int percent = int.Parse(foo);
            
            s = s.Replace(poo,percent+ "% <b>mgk</b>");
        }

       return FirstLetterToUpper(ColourParse(s));
    }

    static string ColourParse(string s)
    {
        if(s.Contains("<b>bleed</b>"))
        {s = s.Replace("<b>bleed</b>","<color=red><b>bleed</b></color>");}

        if(s.Contains("<b>stun</b>"))
        {s = s.Replace("<b>stun</b>","<color=yellow><b>stun</b></color>");}

         if(s.Contains("<b>Stun</b>"))
        {s = s.Replace("<b>Stun</b>","<color=yellow><b>Stun</b></color>");}

        if(s.Contains("<b>phys</b>"))
        {s = s.Replace("<b>phys</b>","<color=orange><b>phys</b></color>");}

       


        if(s.Contains("<b>mgk</b>"))
        {s = s.Replace("<b>mgk</b>","<color=#8C2096><b>mgk</b></color>");}

        return s;
    }

    static string FirstLetterToUpper(string str)
    {
        if (str == null)
            return null;

        if (str.Length > 1)
            return char.ToUpper(str[0]) + str.Substring(1);

        return str.ToUpper();
    }

    public static string Between(string STR , string FirstString, string LastString)
    {       
        string FinalString;     
        int Pos1 = STR.IndexOf(FirstString) + FirstString.Length-1;
        int Pos2 = STR.IndexOf(LastString)+1;
        FinalString = STR.Substring(Pos1, Pos2 - Pos1);
        return FinalString;
    }
}