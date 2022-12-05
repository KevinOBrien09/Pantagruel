using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BeastDisplay : MonoBehaviour
{
    [SerializeField] Image picture;
    [SerializeField] TextMeshProUGUI beastName;
    [SerializeField] List<UIDie> hitDie = new List<UIDie>();
    [SerializeField] List<UIDie>  missDie = new List<UIDie>();
    [SerializeField] List<UIDie>  critDie =new List<UIDie>();
    [SerializeField] List<Sprite> sprites = new List<Sprite>();
    [SerializeField] List<TextMeshProUGUI> statValues = new List<TextMeshProUGUI>();
    Dictionary<int,Sprite> faces = new Dictionary<int, Sprite>();
    [SerializeField] Transform rendertextCam;

    void Awake()
    {
        for (int i = 0; i < 6; i++)
        {
            var n = i;
            faces.Add(n,sprites[n]);
        }
 
    }
    
    public void Apply(Beast b)
    {
        picture.sprite = b.data.uiPicture;
        beastName.text = b.data.beastName;
        DieResults(b.data.pips);
        ApplyStats(b.data.stats,b.statusEffectHandler.modifications);
        Vector3 v =  rendertextCam.position;
        float y = b.data.renderCamY;
        if(y == 0){
            y = 2.3f;
        }
        rendertextCam.position =  new Vector3(v.x,y,v.z);
    }

    public void ApplyStats(Stats baseStats,Stats modifications)
    {
        int phys = baseStats.physical + modifications.physical;
        int magic =  baseStats.magic + modifications.magic;
        int tough =  baseStats.toughness + modifications.toughness;
        int charisma =  baseStats.charisma + modifications.charisma;
        int resolve =  baseStats.resolve + modifications.resolve;

        resolve = Mathf.Clamp(resolve,-90,90);
        tough = Mathf.Clamp(tough,-90,90);

        statValues[0].text = phys.ToString();
        statValues[1].text = magic.ToString();
        statValues[2].text = tough.ToString() + "%";
        statValues[3].text = charisma.ToString();
        statValues[4].text = resolve.ToString() + "%";
    
          
    }

    void DieResults(List<HitPip> p)
    {
        foreach (var item in hitDie)
        {item.DeActivate();}
        
        foreach (var item in missDie)
        {item.DeActivate();}

        foreach (var item in critDie)
        {item.DeActivate();}

        for (int i = 0; i < p.Count; i++)
        {
           switch(p[i].result)
           {
                case HitResult.Hit:
                foreach (var item in hitDie)
                {
                    if(!item.full)
                    {
                        item.Activate(faces[p[i].number]);
                        break;
                    }
                }
                break;
                
                case HitResult.Miss:
                foreach (var item in missDie)
                {
                    if(!item.full)
                    {
                       item.Activate(faces[p[i].number]);
                        break;
                    }
                }
                break;

                case HitResult.Crit:
                foreach (var item in critDie)
                {
                    if(!item.full)
                    {
                        item.Activate(faces[p[i].number]);
                        break;
                    }
                }
                break;
            }
        }

    }
}
