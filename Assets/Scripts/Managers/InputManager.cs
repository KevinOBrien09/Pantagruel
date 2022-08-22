using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputData
{
    public bool uiCancel;
    public bool space;
    public bool confirm;
 
}

public class InputManager:Singleton<InputManager>                   
{
    public static InputData input = new InputData();

    void Update()
    {
        input.uiCancel = Input.GetMouseButtonDown(1);
        input.space = Input.GetKeyDown(KeyCode.Space);
        input.confirm = Input.GetKeyDown(KeyCode.KeypadEnter)|Input.GetKeyDown(KeyCode.Return);
       
    }

    
}