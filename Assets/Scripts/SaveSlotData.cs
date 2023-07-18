using UnityEngine;
using System;

[Serializable]
public class SaveSlotData
{
    public int scene;
    public int slotNumber;
    public string date;
    public string screenShotPath;
    public string lastLocation;
 
    public SaveSlotData(int newSlotNumber,string newDate,string screenShot, string lastLoc)
    {
        slotNumber = newSlotNumber;
        date = newDate;
        screenShotPath = screenShot;
        lastLocation = lastLoc;
    }
    
}