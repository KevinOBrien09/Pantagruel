using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GaugeTicker : Singleton<GaugeTicker>
{
    public bool sweetSpot;
    [SerializeField] RectTransform rt;
    [SerializeField] RectTransform[] targets;
    [SerializeField] GameObject[] bullseyes;
    [SerializeField] AudioSource source;
    List<Tween> t = new List<Tween>();   
    Coroutine c;
    bool moving; 
    Vector2 ogPos;

    void Start(){
        ogPos = rt.anchoredPosition;
    }

    public void Reset()
    {
        rt.anchoredPosition = ogPos;
    }

    public void SetUp(List<int> b)
    {
        sweetSpot = false;
        foreach (var item in bullseyes)
        { item.SetActive(false); }

        for (int i = 0; i < b.Count; i++)
        {
            bullseyes[b[i]].SetActive(true);
            
        }
    }
    
    public void Move()
    {
        foreach (var item in t)
        {item.Kill();}
        t.Clear();
        moving = true;
        c = StartCoroutine(RealMove());
    }

    IEnumerator RealMove()
    {
        t.Add(rt.DOAnchorPosX(targets[0].anchoredPosition.x,.6f));
        yield return new WaitForSeconds(.6f);
        t.Add(rt.DOAnchorPosX(targets[1].anchoredPosition.x,.6f));
        yield return new WaitForSeconds(.6f);
        Move();
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag.Equals("GaugeBullsEye"))
        {sweetSpot = true;}
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag.Equals("GaugeBullsEye"))
        {sweetSpot = false;}
    }

    public void Stop()
    {
        foreach (var item in t)
        {item.Kill();}
        if(c != null)
        {StopCoroutine(c);}
        moving = false;

        if(sweetSpot){
            source.Play();
        }
    }
    
    void Update()
    {
        if(InputManager.input.space && moving)
        {SkillCaster.inst.Cast();}
    }
    
   
   
}
