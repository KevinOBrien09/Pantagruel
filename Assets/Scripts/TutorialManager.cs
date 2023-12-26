using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using TMPro;
using KoganeUnityLib;

[System.Serializable]
public class TutorialProgress
{
    public  GenericDictionary<TutorialEnum,bool> dict = new GenericDictionary<TutorialEnum, bool>();
}
public enum TutorialEnum{BASICS,BASICS2,BASICS3,BASICS4,STATUSEFFECTS,SUMMONS}

[System.Serializable]
public class TutorialProgressSaveable{
    public TutorialEnum stage;
    public bool completed;
}

public class TutorialManager: Singleton<TutorialManager>
{
    public TMP_Typewriter typewriter;
    public TutorialProgress tutorialProgress;
    public GenericDictionary<TutorialEnum,Dialog> tutorialDict = new GenericDictionary<TutorialEnum, Dialog>();
    public GenericDictionary<string,TutorialArrow> tutorialArrowDict = new GenericDictionary<string, TutorialArrow>();
    public Dialog currentTutorial;


   
    public bool ExecuteTutorial(TutorialEnum tutorial)
    {
        return !(tutorialProgress.dict[tutorial]);
    }

    public Dialog GetTutorial(TutorialEnum tutorial){
        if(tutorialDict.ContainsKey(tutorial)){
            currentTutorial = tutorialDict[tutorial];
            return tutorialDict[tutorial];
        }
        else{
            Debug.LogAssertion( tutorial + " NOT FOUND!!!");
            return null;
        }
        
    }

    public void ProcessEvent(string t)
    {
        if(t == "@")
        {
             foreach (var item in tutorialArrowDict)
            {
                item.Value.gameObject.SetActive(false);
            }
           // Debug.LogWarning("an empty string has been input to reset tutorial arrows");
            return;
        }

        if(t == "FREEZE")
        {
            DialogManager.inst.freeze = true;
            return;
        }

       
        if(tutorialArrowDict.ContainsKey(t))
        {
            tutorialArrowDict[t].gameObject.SetActive(true);
           
        }
        else{
            Debug.LogAssertion(t+ " NOT FOUND");
        }
  
    }

    public void LeaveFirstBattle(){

    }

    public bool isInBasics3()
    {
        if(currentTutorial == tutorialDict[TutorialEnum.BASICS3]){
            return true;
        }
        return false;
    }

    public void Leave(){
        var myKey = tutorialDict.FirstOrDefault(x => x.Value == currentTutorial).Key;
        tutorialProgress.dict[myKey] = true;
        DialogManager.inst.ClearNameAndDialogText();
        BattleManager.inst.ExitTutorial(myKey);
        //currentTutorial = null;
    }


    public List<TutorialProgressSaveable> Save()
    {
        List<TutorialProgressSaveable> l = new List<TutorialProgressSaveable>();
        foreach (var item in tutorialProgress.dict)
        {
            TutorialProgressSaveable TPS = new TutorialProgressSaveable();
            TPS.stage = item.Key;
            TPS.completed = item.Value;
            l.Add(TPS);
        }
        return l;
    }

    public void Load(List<TutorialProgressSaveable> loadedProgress)
    {
        tutorialProgress.dict.Clear();
        foreach (var item in loadedProgress)
        {
            tutorialProgress.dict.Add(item.stage,item.completed);
        }
      
    }

}