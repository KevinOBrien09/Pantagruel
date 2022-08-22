using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalBeastSlot : MonoBehaviour
{
    public Beast beast;
    public bool isFull;
    public HealthBar healthBar;

    public void Assign(Beast b)
    {
        beast = b;
        isFull = true;
        healthBar.Apply(b);
        b.onHit += healthBar.DelayChange;
    }
}
