using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class CustomScroll:MonoBehaviour
{
    [SerializeField] GridLayoutGroup grid;
    [SerializeField] RectTransform holder;
    int index;
    Dictionary<int,Vector2> dict = new Dictionary<int, Vector2>();
    int maxLength;

    public void Awake()
    {
        int x;
        int y;
        GetColumnAndRow(grid,out y,out x);
        float columnLength = holder.sizeDelta.y/y;
        float halvedHeight = holder.sizeDelta.y/2;
        int peepee = Mathf.CeilToInt(halvedHeight);
        int startingHeight = -peepee;
        maxLength = y;
        int lastHeight = startingHeight;
      
        for (int i = 0; i < y; i++)
        {
            int j = lastHeight+(int)columnLength;
            dict.Add(i,new Vector2(8,j));
            lastHeight = j;
        }
        if(dict.ContainsKey(0)){
   index = 0;
        holder.anchoredPosition = dict[index];
        }
     
    }

    public void Up()
    {
        index--;
        if(dict.ContainsKey(index)){
        holder.anchoredPosition = dict[index];
        }
    }

    public void Down()
    {
        index++;
        if(dict.ContainsKey(index)){
        holder.anchoredPosition = dict[index];
        }
      
    }

    public void LerpUp(float s)
    {
        holder.anchoredPosition = new Vector2(  holder.anchoredPosition.x,  holder.anchoredPosition .y-s);
        holder.anchoredPosition = new Vector2(holder.anchoredPosition.x,Mathf.Clamp(holder.anchoredPosition.y,dict[0].y,dict[maxLength-1].y));
    }

    public void LerpDown(float s)
    {
        holder.anchoredPosition = new Vector2(  holder.anchoredPosition.x,  holder.anchoredPosition .y+s);
        holder.anchoredPosition = new Vector2(holder.anchoredPosition.x,Mathf.Clamp(holder.anchoredPosition.y,dict[0].y,dict[maxLength-1].y));
    }
    void GetColumnAndRow(GridLayoutGroup glg, out int column, out int row)
    {
        column = 0;
        row = 0;

        if (glg.transform.childCount == 0)
            return;
        column = 1;
        row = 1;
        RectTransform firstChildObj = glg.transform.
        GetChild(0).GetComponent<RectTransform>();
        Vector2 firstChildPos = firstChildObj.anchoredPosition;
        bool stopCountingRow = false;
        for (int i = 1; i < glg.transform.childCount; i++)
        {
            RectTransform currentChildObj = glg.transform.
            GetChild(i).GetComponent<RectTransform>();
            Vector2 currentChildPos = currentChildObj.anchoredPosition;
            if (firstChildPos.x == currentChildPos.x)
            {
                column++;
                stopCountingRow = true;
            }
            else
            {
                if (!stopCountingRow)
                row++;
            }
        }
    }
    
}