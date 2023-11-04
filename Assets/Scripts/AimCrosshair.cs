using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AimCrosshair : MonoBehaviour
{
    public float speed;
    public void Activate(Transform tr)
    {
        gameObject.SetActive(true);
        transform.position = new Vector3(tr.position.x,tr.position.y+.2f,tr.position.z);
    }
    public void Deactivate()
    {
        transform.position = Vector2.zero;
        gameObject.SetActive(false);
    }

    void Update()
    {transform.Rotate (new Vector3 (0, 0, 20) * speed * Time.deltaTime);}
}
