using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectManager : MonoBehaviour
{
    public StatusEffectBehaviour prefab;
    public RectTransform holder;
    public ContentSizeFitter sizeFitter;
    List<StatusEffectBehaviour> statusEffectBehaviours = new List<StatusEffectBehaviour>();
    float ogX;
 
    void Start()
    {
        ogX = holder.sizeDelta.x;
    }

    public void CreateStatusEffect(Card card,StatusEffectEffect effect)
    {
        StatusEffectBehaviour b = Instantiate(prefab,holder);
        statusEffectBehaviours.Add(b);
        b.Init(card.picture,effect);
        if(statusEffectBehaviours.Count > 5){
            sizeFitter.enabled = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
       

        if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            StatusEffectBehaviour b = statusEffectBehaviours[0];
            statusEffectBehaviours.Remove(b);
            Destroy(b.gameObject);

            if(statusEffectBehaviours.Count <= 5){
                sizeFitter.enabled = false;
                holder.sizeDelta = new Vector2(ogX,holder.sizeDelta.y);
            }   
        }
    }
}
