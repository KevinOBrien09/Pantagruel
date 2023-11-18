using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHandler : MonoBehaviour
{
    public List<StatusEffectDisplay> displays = new List<StatusEffectDisplay>();
    public StatusEffectDisplay displayPrefab;
    Vector4 v ;
    public List<StatusEffects> currentEffects = new List<StatusEffects>();
    public Beast owner;

    public void Init(Beast b,int layer)
    {
        gameObject.layer = layer;
        foreach (Transform g in GetComponentsInChildren<Transform>())
        {g.gameObject.layer = layer;}
        v = new Vector4();
        if(BattleManager.inst.GetBeastOwnership(b) == EntityOwnership.PLAYER)
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

    public void CreateStack(StatusEffectEffect so,EffectArgs args)
    {
        StatusEffectDisplay d=   CreateNewStack();
        d.args = args;
          d.statusEffectHandler = this;
        d.Init(so);
        
      
        currentEffects.Add(so.statusEffect);
    }

    public void ClearAllBleeds()
    {
        List<StatusEffectDisplay> d = new List<StatusEffectDisplay>(displays);
        foreach (var item in d)
        {
            if(item.statusEffect == StatusEffects.BLEED){
                item.Kill();
            }
            
        }
    }

    public bool ContainsABleed()
    {
        foreach (var item in displays)
        {
            if(item.statusEffect == StatusEffects.BLEED)
            {return true;}
        }
        return false;
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
}