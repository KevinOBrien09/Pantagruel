using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniGameResultHandler : MonoBehaviour
{
    public int digitalRoot;
    public string display;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] TextMeshProUGUI headerText;

    public void CalculateDigitalRoot(List<string> numbersAsString )
    {
        string n = "";
        foreach (var item in numbersAsString)
        {n = n +item;}
        int num = int.Parse(n);
        digitalRoot = Maths.GetDigitalRoot(num);
        Maths.IsPrime(digitalRoot);
        
        display = RomanNumerals.ToRoman(digitalRoot);
        resultText.text =display;
        if(BattleManager.inst.currentBattleState == BattleManager.BattleState.PlayerTurn){
        SkillDisplay.inst.ParseDesc();
        }
       
    }

    public void ChangeHeader(string newHeader){
    headerText.text =  newHeader;
    }

    public void DisplayCoinResult(CoinResult coinResult)
    {
     
        if(coinResult == CoinResult.Heads){
            resultText.text = "H";
        }
        else{
             resultText.text = "T";
        }

    }

    public void QuestionMark(bool changeSkillDisplay)
    {
        display = "?";
        resultText.text = display;
        if(changeSkillDisplay){
        SkillDisplay.inst.ParseDesc();
        }
     
     
    }



}