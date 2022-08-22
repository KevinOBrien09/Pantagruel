using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Sprites", menuName ="PlayerSprites")]
public class PlayerSpriteSet: ScriptableObject
{
    public PlayerCharacter character;
   public Sprite backSprite;
}