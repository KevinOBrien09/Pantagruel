using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputData
{
    public bool rightClick;
    public bool space;
    public bool confirm;
    public Vector2 WASD;
    public Vector2 mouseDelta;
    public List<bool> wasdDown;
    public bool rotateLeft,rotateRight;
    public bool torch;
}

public class InputManager:Singleton<InputManager>                   
{
    public static InputData input = new InputData();
    void Start()
    {
        input.wasdDown = new List<bool>();
        for (int i = 0; i < 4; i++)
        {
            input.wasdDown.Add(false);
        }
    
       
    }

    void Update()
    {
        input.rightClick = Input.GetMouseButtonDown(1);
        input.space = Input.GetKeyDown(KeyCode.Space);
        input.confirm = Input.GetKeyDown(KeyCode.KeypadEnter)|Input.GetKeyDown(KeyCode.Return);
        input.WASD = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        input.mouseDelta = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
        input.wasdDown[0] = Input.GetKey(KeyCode.W);
        input.wasdDown[1] = Input.GetKey(KeyCode.A);
        input.wasdDown[2] = Input.GetKey(KeyCode.S);
        input.wasdDown[3] = Input.GetKey(KeyCode.D);
        input.rotateLeft = Input.GetKeyDown(KeyCode.Q);
        input.rotateRight = Input.GetKeyDown(KeyCode.E);
        input.torch =  Input.GetKeyDown(KeyCode.T);
    }

    
}