using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton:MonoBehaviour                
{
    public void Move(int dir)
    {
        PlayerManager.inst.movement.StartMove((Dir)dir);
    }

    public void Rotate(int rot)
    {
        PlayerManager.inst.movement.rotate.StartRotate((Dir)rot);
    }

}