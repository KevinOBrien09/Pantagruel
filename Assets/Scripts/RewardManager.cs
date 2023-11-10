using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using TMPro;

public class RewardManager : Singleton<RewardManager>
{
    public RectTransform partyHolderParent,resultsHeader,resultsHeaderBG;
    public TextMeshProUGUI cardCounter,instruction,mainGold,newGold;
    public GameObject noCardPopUp;
    public string instructionTextOneCard,instructionTextTwoCard;
    public Transform partyHolder,cardHolder;
    public PartyEXPRewardDisplay partyPrefab;
    public ShopCard cardPrefab;
    public List<Card> cardsToAddToCollection = new List<Card>();
    public int howManyRewardCards = 1;
    List<Card> openCards = new List<Card>();
    List<PartyEXPRewardDisplay> partyEXPs = new List<PartyEXPRewardDisplay>();
    List<ShopCard> shopCards = new List<ShopCard>();
    public SoundData addGold;
    public int additionalGold;
    void Start()
    {
        gameObject.SetActive(false);
        
    }

    public void Open(){
        if(howManyRewardCards == 1){
            instruction.text  =instructionTextOneCard;
        }
        else{
              instruction.text  =instructionTextTwoCard;
        }

        gameObject.SetActive(true);
        BattleTicker.inst.Type("");
        CardManager.inst.LeaveBattle();
      CardStack.inst.ExitBattle();
        Inventory.inst.DisableItemDragOnAll();
        MusicManager.inst.Reward();
        partyHolderParent.DOAnchorPosX(-170,0);
        resultsHeader.DOAnchorPosX(-65,0);
        resultsHeaderBG.DOAnchorPosX(-65,0);
        partyHolderParent.DOAnchorPosX(264,.33f);
        resultsHeader.DOAnchorPosX(265,.33f);
        resultsHeaderBG.DOAnchorPosX(270,.1f);
        foreach (var item in PlayerParty.inst.party)
        {
            PartyEXPRewardDisplay d = Instantiate(partyPrefab,partyHolder);
            d.Init(item);
            partyEXPs.Add(d);
        }

        StartCoroutine(giveEXP());
        spawnCards();
        IEnumerator giveEXP()
        {
            yield return new WaitForSeconds(.5f);

            int i = 0;
            foreach (var item in PlayerParty.inst.party)
            {
                int oldEXP = item.exp.currentExp;
                int gained = item.exp.targetExp/2;
                
                EXP copy = new EXP();
                copy.level = item.exp.level;
                copy.targetExp = item.exp.targetExp;
                copy.currentExp = item.exp.currentExp;
                partyEXPs[i].SimulateAddExp(gained,copy); //fake
                item.exp.AddExp(gained); //real
                i++;
            }
        }

        void spawnCards()
        {
            CardViewer.inst.manuallyLoadCards = true;
            System.Random rng = new  System.Random();
            var shuffledcards = LocationManager.inst.currentLocation.cardRewards.OrderBy(a => rng.Next()).ToList();
            int howManyCards = 3;
            for (int i = 0; i < howManyCards; i++)
            {
                ShopCard card = Instantiate(cardPrefab,cardHolder);
                card.Init(shuffledcards[i],true);
                openCards.Add(shuffledcards[i]);
                shopCards.Add(card);
            }
            CardViewer.inst.ManuallyLoadCards(openCards);
            
        }

        
        int howMuchGold = (int) Random.Range
        (LocationManager.inst.currentLocation.goldRewardRange.x,LocationManager.inst.currentLocation.goldRewardRange.y); 
      
        int o = Inventory.inst.gold;
        int n = howMuchGold;
              bool a = false;
        StartCoroutine(addGold());
        AudioManager.inst.GetSoundEffect().Play(this.addGold);
        IEnumerator addGold()
        {
            newGold.text = "+"+n;
            mainGold.text = "Coin:" + o;
            yield return null;
            o++;
            n--;
            if(n >= 0)
            {
                StartCoroutine(addGold());
               
            }
            else
            {
                if(!a){
                    if(additionalGold != 0)
                    {
                        n = additionalGold;
                        newGold.color = Color.red;
                        a = true;
                        StartCoroutine(addGold());
                    }
                }
                else{
                    newGold.color = Color.white;
                }
               
            }
        }
  int t = howMuchGold += additionalGold;
        Inventory.inst.AddGold(t);

    }

    public void Proceed()
    {
        if(cardsToAddToCollection.Count ==0)
        {noCardPopUp.SetActive(true);}
        else{Leave();}
    }

    public void ClosePopUp(){
        
        noCardPopUp.SetActive(false);
     
        
    }

    public void Leave(){
        foreach (var item in cardsToAddToCollection)
        {
            CardCollection.inst.AddCard(item);
        }

        foreach (var item in shopCards)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in partyEXPs)
        {
            Destroy(item.gameObject);
        }
        shopCards.Clear();
        partyEXPs.Clear();
        cardsToAddToCollection.Clear();
        openCards.Clear();
        gameObject.SetActive(false);
        CardViewer.inst.manuallyLoadCards = false;
        BattleManager.inst.LeaveBattle();
        ClosePopUp();
    }

    public void RefreshCardList(Card c){
        if(cardsToAddToCollection.Contains(c))
        {
            cardsToAddToCollection.Remove(c);
        }
        else{
            cardsToAddToCollection.Add(c);
        }
        cardCounter.text = cardsToAddToCollection.Count + "/" + howManyRewardCards.ToString();
        if(cardsToAddToCollection.Count == howManyRewardCards)
        {
            foreach (var item in shopCards)
            {
                if(!item.isSelected)
                {item.Disable();}
                // else
                // {item.Enable(); }
               
                
            }
        }
        else{
            foreach (var item in shopCards)
            {
               item.Enable();
            }
        }

    }
}
