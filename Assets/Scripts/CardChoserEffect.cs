using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class CardChoserEffect :Effect
{

    public virtual void Selected(Card card)
    {

    }

    public override void Use(EffectArgs args)
    {
       // CardManifester.inst.Open(cardsToManifest);
       CardManifester.inst.Open(this);
    }

	public override bool canUse(bool isPlayer){

		if(CardManager.inst. hand.Count < 7)
        {return true;
		}
		return false;
	}

   

}