 
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
            s = s.Replace(poo,"<color=#0060FF>" + amount.ToString() + "</color>");
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
        if(s.Contains("bleed"))
        {s = s.Replace("bleed","<color=red><b>bleed</b></color>");}

        if(s.Contains("stun"))
        {s = s.Replace("stun","<color=yellow><b>stun</b></color>");}

        if(s.Contains("Stun"))
        {s = s.Replace("Stun","<color=yellow><b>Stun</b></color>");}

        if(s.Contains("phys"))
        {s = s.Replace("phys","<color=orange><b>phys</b></color>");}
        
        if(s.Contains("mgk"))
        {s = s.Replace("mgk","<color=#0060FF><b>mgk</b></color>");}

        if(s.Contains("parasite"))
        {s = s.Replace("parasite","<color=#FF00BD><b>parasite</b></color>");}
        
        if(s.Contains("guard break"))
        {s = s.Replace("guard break","<color=#286326><b>Guard Break</b></color>");}
        else if (s.Contains("guard"))  {s = s.Replace("guard","<color=#286326><b>Guard</b></color>");}

        if(s.Contains("blockade"))
        {s = s.Replace("blockade","<color=#143267><b>Blockade</b></color>");}

        if(s.Contains("raw"))
        {s = s.Replace("raw","<b>raw</b>");}

        // if(s.Contains("heal"))
        // {s = s.Replace("heal","<color=#048263><b>Heal</b></color>");}

         if(s.Contains("destroy"))
        {s = s.Replace("destroy","<color=black><b>Destroy</b></color>");}

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