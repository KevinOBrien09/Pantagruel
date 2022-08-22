using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalBeastSpawner : MonoBehaviour
{
   public List<RivalBeastSlot> slots = new List<RivalBeastSlot>();
   public List<BeastScriptableObject> beastPool = new List<BeastScriptableObject>();
   public List<BeastScriptableObject> fullPool = new List<BeastScriptableObject>();
   public bool USE_FULL_POOL;
   [SerializeField] GameObject beastPrefab;

    public List<Beast> Spawn(List<BeastData> predterminedBattle =  null)
    {
        List<Beast> beasts = new List<Beast>();
        if(predterminedBattle != null)
        {
           Debug.Log("not implemented");
        }
        else
        {
            foreach (var item in slots)
            {
                GameObject g = Instantiate(beastPrefab,item.transform);
                Beast b = g.GetComponent<Beast>();
                BeastScriptableObject bso = null;
                if(USE_FULL_POOL)
                {bso = fullPool[Random.Range(0,fullPool.Count)];}
                else
                {bso = beastPool[Random.Range(0,beastPool.Count)];}
               
                b.Init(bso,Alliance.Enemy);
                item.Assign(b);
                beasts.Add(b);
            }
        }

        return beasts;
    }



}