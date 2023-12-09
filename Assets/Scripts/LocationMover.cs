using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LocationMover: Interactable
{
    public MainLocation newMainLocation;
    public Location newSubLocation;
    public Vector3 playerPos,playerRot;

    public override void Go()
    {
        if(newMainLocation != null)
        {
            LocationManager.inst.ChangeMainLocation(newMainLocation);
            return;
        }
        else if(newSubLocation != null)
        {
            LocationManager.inst.ChangeSubLocationWithFade(newSubLocation,playerPos,playerRot);
        }
        
        else
        {
            Debug.Log("tHIS DOES NOTHING?");
        }
        
       

    }


}