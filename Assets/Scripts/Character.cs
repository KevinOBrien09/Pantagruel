using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Dialog/Character", fileName = "Character")]
public class Character:ScriptableObject
{
    public string characterName;
    public Color32 charColour;

}