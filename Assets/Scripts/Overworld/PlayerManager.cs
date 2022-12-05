using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerManager : MonoBehaviour
{
	public PlayerCharacter character;
	[SerializeField] SpriteRenderer back;
	[SerializeField] PlayerSpriteSet spriteSet;
	float ogY;

	public void OnEnable()
	{
		ogY = transform.position.y;
		ApplyPlayerSprite();
		IdleBounce();
		
	}

	public void ApplyPlayerSprite()
	{
		character = spriteSet.character;
		back.sprite = spriteSet.backSprite;
    }


	void IdleBounce(){
		transform.DOMoveY(ogY+.33f,2).OnComplete(()=> transform.DOMoveY(ogY,2).OnComplete(()=>IdleBounce()));
	}

}