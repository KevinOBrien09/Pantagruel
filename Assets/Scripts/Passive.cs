
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PassiveOperativeState{OperatesOnlyWhenActiveBeast,AlwaysOn,OperatesOnlyWhenInBackline}
[CreateAssetMenu(menuName = "Beast Passive/NA")]
public class Passive : ScriptableObject
{
    public string passiveName;
    [TextArea(20,20)] public string passiveDesc;
  
    
    public PassiveOperativeState operativeState;

    public virtual void Init(Beast b)
    {
Debug.Log(b.scriptableObject.beastData.beastName + " passive has been initalized");
    }
    
    public virtual void Wipe(Beast b){
Debug.Log(b.scriptableObject.beastData.beastName + " passive has been wiped");
    }

 

}