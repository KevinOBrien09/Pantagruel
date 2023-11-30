using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using TMPro;

public class CardManifester:Singleton<CardManifester>                   
{      
    public bool isManifesting;
    public GameObject cards,manifestHolder;
    public  Transform cardHolder;
    public CanvasGroup mHold;
    public SoundData manifestOpen;
    public ManifestCardBehaviour cardPrefab;
    public List<Card> currentSelection = new List<Card>();
    List<GameObject> cb = new List<GameObject>();
    public List<GameObject> manifestNerdShit = new List<GameObject>();
    public CardChoserEffect cardChoserEffect;
    public TextMeshProUGUI txt;
    public void Open(CardChoserEffect choserEffect)
    {
        CardManager.CardState state = CardManager.CardState.NORMAL;
        ManifestCardsEffect m = choserEffect as ManifestCardsEffect;
        DrawCardAtSpecifiedTurn d = choserEffect as DrawCardAtSpecifiedTurn;
        foreach (var item in manifestNerdShit)
        {item.gameObject.SetActive(false);}

        if(m != null)
        {
            currentSelection = new List<Card>(m. cardsToManifest);
            foreach (var item in manifestNerdShit)
            {item.gameObject.SetActive(true);}
            txt.text = "Conjuring Card";
            state = CardManager.CardState.MANIFESTED;
        }

        if(d != null)
        {
            txt.text = "Pick a Card";
            List<Card> c = new List<Card>();
            for (int i = 0; i < d.howManyCardsToChoseFrom; i++)
            {
                Card cd = CardManager.inst.DrawCard();
                if(cd != null){
                    c.Add(cd);
                }
                currentSelection = new List<Card>(c);

            }
        }


        cardChoserEffect = choserEffect;
        isManifesting = true;
        BattleManager.inst.FUCKOFF = true;
        CardManager.inst.HideHand();
      
        EndTurnButton.inst.Deactivate();
        StartCoroutine(q());
        IEnumerator q()
        {
            mHold.alpha = 0;
            foreach (var item in currentSelection)
            {
                ManifestCardBehaviour m = Instantiate(cardPrefab,cardHolder);
                m.Init(item,state);
                cb.Add(m.gameObject);
            }
            yield return new WaitForSeconds(.1f);
            manifestHolder.gameObject.SetActive(true);
            AudioManager.inst.GetSoundEffect().Play(manifestOpen);
            mHold.DOFade(1,.1f);
            Debug.Log("Spaw cards");
        }
    
    }

    public void SelectedCard(Card card){
       
        Close();
        cardChoserEffect.Selected(card);
        DrawCardAtSpecifiedTurn d =  cardChoserEffect as DrawCardAtSpecifiedTurn;
        if(d != null){
        currentSelection.Remove(card);
            foreach (var item in currentSelection)
            {
                CardManager.inst.currentDeck.AddCardToDeck(item);
            }
        }
        cardChoserEffect = null;
       
    }

    public void Close()
    {
        mHold.DOFade(1,.2f).OnComplete(()=>
        {
        
            manifestHolder.gameObject.SetActive(false);
            for (int i = 0; i < cb.Count; i++)
            {Destroy(cb[i].gameObject);}
            cb.Clear();
            isManifesting = false;
            BattleManager.inst.FUCKOFF = false;
            StartCoroutine(q());
            IEnumerator q(){
                CardManager.inst.DeactivateHand();
                yield return new WaitForSeconds(.5f);
                CardManager.inst.ActivateHand();
                EndTurnButton.inst.Reactivate();
            }
          

        });
     
    }

}