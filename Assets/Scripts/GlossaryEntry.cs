using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GlossaryEntry: MonoBehaviour                  
{
    public TextMeshProUGUI g_name,desc;

    public void Apply(GlossaryDefinition definition){
        g_name.text = definition.glossName;
        desc.text = definition.desc;
    }
}