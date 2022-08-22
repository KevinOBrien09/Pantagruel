using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class CheckSkillCost
{

    public static int MinCost(Beast b)
    {
        if(b == null)
        {
            Debug.LogAssertion("No beast");
            return 0;
        }

        if(b.data.skills.Count > 0)
        {
            List<int> count = new List<int>();
            for (int i = 0; i < b.data.skills.Count; i++)
            { count.Add( b.data.skills[i].skillData.cost.amount);}
            return count.Min();
        }
        else
        {
            Debug.LogWarning("No Skills here..");
            return 0;
        }
       
    }

    

    public static bool canCast(Beast b,Skill s)
    {
        SkillResource sr = s.skillData.cost;
        switch (sr.resource)
        {
            case ResourceCurrency.SP:

            if(b.currentSP >= sr.amount)
            {return true;}
            else
            {return false;}
          
            case ResourceCurrency.HP:
            if(b.currentHealth >= sr.amount)
            {return true;}
            else
            {return false;}
        
            
            default:
            Debug.LogAssertion("Default Switch");
            return false;
        }
    }


}