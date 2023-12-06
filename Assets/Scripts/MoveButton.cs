using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveButton:MonoBehaviour                
{
    public void Move(int dir)
    {
        PlayerManager.inst.movement.StartMove((Dir)dir);
         EventSystem.current.SetSelectedGameObject(null);
    }

    public void Rotate(int rot)
    {
        PlayerManager.inst.movement.rotate.StartRotate((Dir)rot);
        EventSystem.current.SetSelectedGameObject(null);
    }

}