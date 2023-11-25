using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHandler : MonoBehaviour
{
    public List<StatusEffectDisplay> displays = new List<StatusEffectDisplay>();
    public StatusEffectDisplay displayPrefab;
    Vector4 v ;
    public List<StatusEffects> currentEffects = new List<StatusEffects>();
    public List<StatusEffectEffect> allEffects = new List<StatusEffectEffect>();
    public Beast owner;
    public Dictionary<Card,StatusEffectDisplay> parasiteStacks = new Dictionary<Card, StatusEffectDisplay>();

    public void Init(Beast b,int layer)
    {
        gameObject.layer = layer;
        foreach (Transform g in GetComponentsInChildren<Transform>())
        {g.gameObject.layer = layer;}
        v = new Vector4();
        if(b.OwnedByPlayer())
        {v = b.scriptableObject.beastData.playerStatusEffectPos;}
        else{v = b.scriptableObject.beastData.enemyStatusEffectPos;}
        this.transform.localPosition = new Vector3(v.x,v.y,v.z);
        b.statusEffectHandler = this;
        owner= b;
    }

    public StatusEffectDisplay CreateNewStack()
    {
       
        StatusEffectDisplay d = Instantiate(displayPrefab,transform);
        d.gameObject.layer = this.gameObject.layer;
        foreach (Transform item in d.transform)
        { item.gameObject.layer = this.gameObject.layer; }
        
        displays.Add(d);
        MakeCircular(v.w);
        return d;
    }

    public StatusEffectDisplay CreateStack(StatusEffects so)
    {
        StatusEffectDisplay d=   CreateNewStack();
  

        d.Init(so,this);
        currentEffects.Add(so);
        

        return d;
    }

    public void ClearAllBleeds()
    {
        List<StatusEffectDisplay> d = new List<StatusEffectDisplay>(displays);
        foreach (var item in d)
        {
            if(item.statusEffect == StatusEffects.POISON){
                item.Kill();
            }
            
        }
    }
    
    public bool ContainsThisEffect(StatusEffects effects)
    {
        foreach (var item in displays)
        {
            if(item.statusEffect == effects)
            {return true;}
        }
        return false;
    }

    public void RemoveParasiteStack()
    {
        if(ContainsThisEffect(StatusEffects.PARASITE))
        {
            StatusEffectDisplay D = null;
            foreach (var item in displays)
            {
                if(item.statusEffect == StatusEffects.PARASITE)
                {
                    D = item;
                    break;
                }
            }
            D.Kill();
        }
    }

     
    
    public void MakeCircular(float radius)
    {
        for (int i = 0; i < displays.Count; i++)
        {
            var radians = 2 * Mathf.PI / displays.Count * i;
            var vertical = Mathf.Sin(radians);
            var horizontal = Mathf.Cos(radians);
            var spawnDir = new Vector3(horizontal, 0, vertical);
            var spawnPos = Vector3.zero + spawnDir * radius;
            displays[i].transform.localPosition = spawnPos;

        }
    }


    public List<StatusEffects> Save(){
        return new List<StatusEffects>(currentEffects);
    }

    public void Load(List<StatusEffects> effects){
        
        foreach (var item in effects)
        {
            switch(item){
                case StatusEffects.PARASITE:
                    CreateStack(item).AddStatusEffectCardToDeck(allEffects[1].cards);
                break;
                default:
                   CreateStack(item);

                break;
            }
          
        }
    }
}