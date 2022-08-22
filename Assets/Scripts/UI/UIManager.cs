using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager:Singleton<UIManager>                   
{
    public enum UIState{Normal,Inventory,Swap,Strat,Hidden}
    public enum SideState{Normal,Cast,Hide}
    public UIState currentUIState;
    public SideState currentSideState;
    public GameObject[] uiStates;
    public GameObject[] sideStateGOS;
    Dictionary<UIState,GameObject> mainStates = new Dictionary<UIState, GameObject>();
    Dictionary<SideState,GameObject> sideStates = new Dictionary<SideState, GameObject>();
    
    void Start()
    {
        mainStates.Add(UIState.Normal,uiStates[0]);
        mainStates.Add(UIState.Inventory,uiStates[1]);
        mainStates.Add(UIState.Swap,uiStates[2]);
        mainStates.Add(UIState.Strat,uiStates[3]);
        mainStates.Add(UIState.Hidden,uiStates[4]);

        sideStates.Add(SideState.Normal,sideStateGOS[0]);
        sideStates.Add(SideState.Cast,sideStateGOS[1]);
        sideStates.Add(SideState.Hide,sideStateGOS[2]);
        Normal();
    }

    void SwitchState(UIState newState)
    {
        foreach (var item in mainStates)
        {item.Value.SetActive(false);}
        currentUIState = newState;
        mainStates[currentUIState].SetActive(true);
    }

    void SwitchSideState(SideState newState)
    {
        foreach (var item in sideStates)
        {item.Value.SetActive(false);}
        currentSideState = newState;
        sideStates[currentSideState].SetActive(true);
    }
    
    public void Normal()
    {SwitchState(UIState.Normal);
    SwitchSideState(SideState.Normal);
    }

    public void Inventory()
    {SwitchState(UIState.Inventory);}

    public void Swap()
    {SwitchState(UIState.Swap);}

    public void Strat()
    {SwitchState(UIState.Strat);}

    public void Hide()
    {SwitchState(UIState.Hidden);}

    public void SideNormal()
    {SwitchSideState(SideState.Normal);}
    
    public void SideCast()
    {SwitchSideState(SideState.Cast);}

    public void SideHide()
    {SwitchSideState(SideState.Hide);}
    
    void Update()
    {
        if(InputManager.input.uiCancel)
        {
            switch (currentUIState)
            {
                case UIState.Normal:
                break;
                case UIState.Inventory:
                Normal();
                break;
                case UIState.Swap:
                Normal();
                break;
                case UIState.Strat:
                Normal();
                break;
                case UIState.Hidden:

                break;
                
            }

        }
    }

    
}