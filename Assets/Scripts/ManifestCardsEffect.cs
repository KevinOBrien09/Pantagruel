using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/ManifestCardsEffect", fileName = "ManifestCardsEffect")]
public class ManifestCardsEffect : CardChoserEffect
{

    public List<Card > cardsToManifest = new List<Card>();
    public override void Selected(Card card)
    {CardManager.inst.AddManifestedCardToDeck(card);}

	
   

}