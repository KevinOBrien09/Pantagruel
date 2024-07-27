using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "PetEffect")]
public class PetEffect : ScriptableObject
{
    public List<Effect> effects = new List<Effect>();
   [TextArea(5,5)] public string desc;

}