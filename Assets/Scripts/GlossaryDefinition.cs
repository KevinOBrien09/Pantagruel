using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(menuName = "GlossaryDefinition", fileName = "GlossaryDefinition")]
public class GlossaryDefinition : ScriptableObject
{
    public string glossName;
   [TextArea(20,20)] public string desc;
}