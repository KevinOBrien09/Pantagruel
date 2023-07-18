using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Biome
{
    Forest,Water,Dungeon,WoopsieDoopsie
}

public class Floor : MonoBehaviour
{
    public Biome tileType;
}
