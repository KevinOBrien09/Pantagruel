using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DungeonTextInput : MonoBehaviour
{
    public TextMeshProUGUI log;
    public TMP_InputField inputField;
    public string logString;

    void Start()
    {UpdateLog();}
    void Update()
    {
        if(InputManager.input.confirm)
        {
            if(!String.IsNullOrWhiteSpace(inputField.text))
            {CheckIfValid();}
        }
    }

    public void CheckIfValid()
    {
        string input = inputField.text.ToLower();
        (string,bool) sneed = DungeonManager.inst.CheckEverPresent(input);
        if(sneed.Item2)
        {
            if(sneed.Item1 != string.Empty){
                logString += "<br><color=red>" +sneed.Item1 + "</color>";
                UpdateLog();
            }
         
            goto end;
        }

        foreach (ExplorationAction action in DungeonManager.inst.currentRoom.options)
        {
            foreach (string prompt in action.validResponse)
            {
                if(input.Contains(prompt))
                {
                    logString += "<br><color=red>" + action.logString + "</color>";
                    UpdateLog();
                    DungeonManager.inst.ParseAction(action);
                    goto end;
                }
                
            }
        }
        end:
        inputField.text = string.Empty;
        inputField.ActivateInputField();
    }
    public void UpdateLog()
    {log.text = logString;}
    
}
