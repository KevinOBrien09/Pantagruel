using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using KoganeUnityLib;
using UnityEngine.UI;
using DG.Tweening;

public class StarterSelectorObject : MonoBehaviour
{
    public GenericDictionary<BeastScriptableObject,Sprite> beastOrbPair = new GenericDictionary<BeastScriptableObject, Sprite>();
    public List<Transform> holders = new List<Transform>();
    public List<StarterSelectorClicker> clickers = new List<StarterSelectorClicker>();
    public Camera cam;
    public Color32 grey,lightGrey;
    public TMP_Typewriter typewriter,desc;
    Dictionary<BeastScriptableObject,Transform> clickerDict = new Dictionary<BeastScriptableObject, Transform>();
    Vector3 left,right;
    Vector3  center = new Vector3(0,0,-2);
    public BeastScriptableObject selectedBeast;
    public int currentIndex = -99;
    public GenericDictionary<BeastScriptableObject,string> descs = new GenericDictionary<BeastScriptableObject, string>();
    public void Init(List<StarterSelectorClicker> c)
    {
        clickers = new List<StarterSelectorClicker>(c);
        for (int i = 0; i < beastOrbPair.Count; i++)
        {
            holders[i].GetChild(0).GetComponent<SpriteRenderer>().sprite = beastOrbPair.ElementAt(i).Value;
            BeastScriptableObject bso = beastOrbPair.ElementAt(i).Key;
            GameObject visual =  Instantiate(bso.beastData.beastGraphicPrefab,holders[i]);
            
            visual.transform.localPosition = Vector3.zero;

            foreach (Transform item in MiscFunctions.GatherAllTransforms(visual.transform,new List<Transform>()))
            {
                item.gameObject.layer = gameObject.layer;
            }
            clickers[i].starterSelectorObject = this;
            clickers[i].Init(visual,bso);
            ChangeBeastSpriteColour(visual,grey);
            clickerDict.Add(bso,clickers[i].starterVisual.transform.parent);
        }

        left = clickers[2].ogPos;
        right = clickers[1].ogPos;
    }

   
  
    public void ChangeBeastSpriteColour(GameObject bo,Color c)
    {
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
        foreach (Transform item in MiscFunctions.GatherAllTransforms(bo.transform,new List<Transform>()))
        {
            SpriteRenderer s = null;
            if(item.TryGetComponent<SpriteRenderer>(out s))
            {spriteRenderers.Add(s);}
        }

        foreach (var item in spriteRenderers)
        {item.color = c;}
    }

    public void FirstMove(BeastScriptableObject selectedBeast)
    {
        foreach (var item in clickers)
        {item.gameObject.SetActive(false);}
        if(clickerDict.ElementAt(1).Key == selectedBeast)
        {currentIndex = 1;}
        else if(clickerDict.ElementAt(2).Key == selectedBeast)
        {currentIndex = 2;}
        else
        {currentIndex = 0;}
        Move(); 
        desc.Play(descs[selectedBeast],50,()=>{});
        typewriter.Play(selectedBeast.beastData.beastName,30,()=>{});
         this.selectedBeast = selectedBeast;
    }

    public void BeastSelected(BeastScriptableObject selectedBeast)
    {
        this.selectedBeast = selectedBeast;
        foreach (var item in clickerDict)
        {
            ChangeBeastSpriteOrder(clickerDict[item.Key].GetChild(1).gameObject,"Default");
            ChangeBeastSpriteColour(clickerDict[item.Key].GetChild(1).gameObject,grey);
        }
        ChangeBeastSpriteOrder(clickerDict[this.selectedBeast].GetChild(1).gameObject,"Pet");
        ChangeBeastSpriteColour(clickerDict[this.selectedBeast].GetChild(1).gameObject,Color.white);
        typewriter.Play(selectedBeast.beastData.beastName,30,()=>{});
        desc.Play(descs[selectedBeast],50,()=>{});
    }

    public void Left()
    {
        if(currentIndex == 0)
        {currentIndex = 1;}
        else if(currentIndex == 1)
        {currentIndex = 2;}
        else if(currentIndex == 2)
        { currentIndex = 0; }
        
       BeastSelected(Move());
    }

    public void Right(){

        if(currentIndex == 0)
        {currentIndex = 2;}
        else if(currentIndex == 1)
        {currentIndex = 0;}
        else if(currentIndex == 2)
        {currentIndex = 1;}
        BeastSelected(Move());
    }

    public BeastScriptableObject Move()
    {
        ToggleAllBones(false);
        if(currentIndex == 0){ //inquis
            clickerDict[beastOrbPair.ElementAt(0).Key].DOLocalMove(center,.25f).OnComplete(()=>{
                ToggleAllBones(true);
            });
             clickerDict[beastOrbPair.ElementAt(2).Key].DOLocalMove(left,.25f);
              clickerDict[beastOrbPair.ElementAt(1).Key].DOLocalMove(right,.25f);
             return   selectedBeast = beastOrbPair.ElementAt(0).Key;
        }
        else if(currentIndex == 1){ //dame
            clickerDict[beastOrbPair.ElementAt(1).Key].DOLocalMove(center,.25f).OnComplete(()=>{
                ToggleAllBones(true);
            });
            clickerDict[beastOrbPair.ElementAt(0).Key].DOLocalMove(left,.25f);
            clickerDict[beastOrbPair.ElementAt(2).Key].DOLocalMove(right,.25f);
           return selectedBeast = beastOrbPair.ElementAt(1).Key;
        }
        else if(currentIndex == 2){ //seeker
            clickerDict[beastOrbPair.ElementAt(2).Key].DOLocalMove(center,.25f).OnComplete(()=>{
                ToggleAllBones(true);
            });
            clickerDict[beastOrbPair.ElementAt(1).Key].DOLocalMove(left,.25f);
            clickerDict[beastOrbPair.ElementAt(0).Key].DOLocalMove(right,.25f);
           return beastOrbPair.ElementAt(2).Key;
         
        }
        Debug.LogAssertion("NO BEAST RETURNED");
        return null;    
      

      
    }

    public void ToggleAllBones(bool state){
        foreach (var item in clickers)
        {
            item.ToggleSpringBones(state);
        }
    }


    public void ChangeBeastSpriteOrder(GameObject bo,string i)
    {
        List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
        foreach (Transform item in MiscFunctions.GatherAllTransforms(bo.transform,new List<Transform>()))
        {
            SpriteRenderer s = null;
            if(item.TryGetComponent<SpriteRenderer>(out s)){
                spriteRenderers.Add(s);
            }
        }

        foreach (var item in spriteRenderers)
        {
            item.sortingLayerName = i;
        }
    }

}