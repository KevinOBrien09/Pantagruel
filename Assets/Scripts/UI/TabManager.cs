using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : Singleton<TabManager>
{
    [SerializeField] List<Tab> tabs = new List<Tab>();

    void Start(){
        DeselectAll();
        tabs[0].Clicked();
    }

    public void DeselectAll()
    {
        foreach (var item in tabs)
        {
            
            item.MakeSmall();
        }
    }
}
