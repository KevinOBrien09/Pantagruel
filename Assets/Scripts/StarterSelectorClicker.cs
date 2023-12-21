using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using EasySpringBone;
using System.Linq;

public class StarterSelectorClicker : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public StarterSelectorObject starterSelectorObject;
    public Dialog dialog;
    public GameObject starterVisual;
    public BeastScriptableObject scriptableObject;
	public Vector3 ogPos;
	int behind,forward;
	List<SpringBone> springBones;
    public void Init(GameObject visual,BeastScriptableObject so){
        starterVisual = visual;
		springBones =	visual.GetComponentsInChildren<SpringBone>().ToList();
        scriptableObject = so;
		ogPos = visual.transform.parent.localPosition;
	
    }

    public void OnPointerClick(PointerEventData eventData)
    {
		if(!StarterSelector.inst.firstClick){
			StarterSelector.inst.firstClick = true;
			
			
			starterSelectorObject.FirstMove(scriptableObject);
			StarterSelector.inst.arrows.gameObject.SetActive(true);
			
			StarterSelector.inst.confirm.gameObject.SetActive(true);
		}
		



       	
    }

	public void Return(){
		starterSelectorObject.ChangeBeastSpriteOrder(starterVisual,"Default");
		starterSelectorObject.ChangeBeastSpriteColour(starterVisual,starterSelectorObject.grey);
		starterVisual.transform.parent.DOLocalMove(ogPos,.25f);
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
       starterSelectorObject.ChangeBeastSpriteColour(starterVisual,starterSelectorObject.lightGrey);
     //  starterSelectorObject.typewriter.Play(scriptableObject.beastData.beastName,30,()=>{});
    }

	public void OnPointerExit(PointerEventData eventData)
    {
       starterSelectorObject.ChangeBeastSpriteColour(starterVisual,starterSelectorObject.grey);
       starterSelectorObject.typewriter.Play("",30,()=>{});
    }

	public void ToggleSpringBones(bool state){
  		foreach (var item in springBones)
		{item.ignoreSpringBone = !state;}
    }
}