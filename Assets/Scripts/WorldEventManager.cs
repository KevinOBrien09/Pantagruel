using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WorldEventManager : Singleton<WorldEventManager>
{
    public GenericDictionary<string,WorldEvent> dict = new GenericDictionary<string, WorldEvent>();

    public void ProcessEvent(string id){
        if(dict.ContainsKey(id)){
            dict[id].Go();
        }
        else{
            Debug.LogAssertion(id+ ": Not Found");
        }
    }
}