using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CardBehaviour : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler,IBeginDragHandler,IEndDragHandler,IDragHandler,IInitializePotentialDragHandler
{
    public static bool dragging;
    public Canvas canvas;
    public Card card;
    public RectTransform rt;
    public TextMeshProUGUI cardName;    
    public TextMeshProUGUI manaCost;    

    public GameObject manaGem;
    public CanvasGroup canvasGroup;
    public Image icon;
    public Image paper;
    public Color32 vaporousBlue,manifestedRed;
    public List<Image> imagesToHaveWobbleShader = new List<Image>();
    public List<Image> imagesToHaveDissolveShader = new List<Image>();
    public Material vaporousWobble,vaporousDissolve;
    public float yUp;
    public bool interactable;
    public CardManager.CardState state;
    public GameObject manaIconGO;
    Canvas parentCanvas;
    Vector2 ogPos;
    List<Tween> activeTweens = new List<Tween>();
   
    float moveSpeed = .2f;
    int OGSorting;
    bool up;
   
    
    void Start()
    {
        OGSorting = canvas.sortingOrder;
        parentCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
        ogPos = rt.anchoredPosition;
    }

    public void Init(Card newCard,CardManager.CardState state)
    {
        card = newCard;
        icon.sprite = card.picture;
        cardName.text = card.cardName;
        gameObject.name = card.cardName;
        if(card.manaCost == 0)
        {manaIconGO.SetActive(false);}
        else
        {manaCost.text = RomanNumerals.ToRoman(card.manaCost);}

        switch (state)
        {
            case CardManager.CardState.VAPOUR:
           
            paper.color = vaporousBlue;
            icon.material = vaporousWobble;
            cardName.text = "<i>Vaporous </i>" + card.cardName;
            break;
            case CardManager.CardState.MANIFESTED:
            paper.color = manifestedRed;
            icon.material = vaporousWobble;
            cardName.text = "<i>" + card.cardName;
            break;

            
            default:
            break;
        }
        this.state = state;
    }
    
    public void VaporousDissolve()
    {
        List<Material> m = new List<Material>();
        foreach (var item in imagesToHaveDissolveShader)
        {
            Material mat = Instantiate(vaporousDissolve);
            item.material = mat;
            m.Add(mat);
        } 
        float dissTime = 1.5f;
        foreach (var item in m)
        {
            DOVirtual.Float( item.GetFloat("_DissolvePower"),0,dissTime,v  => 
            { item.SetFloat("_DissolvePower",v);});
        }
       
        StartCoroutine(q());
        IEnumerator q()
        {
            yield return new WaitForSeconds(dissTime);
            foreach (var item in m)
            {
                item.SetFloat("_EmissionThickness",0);
            }
        }
    }

    public void EnableInteractable()
    {
        interactable = true;
        canvas.overrideSorting = true;
    }

    public void DisableInteractable()
    {   canvas.sortingOrder = OGSorting;
        interactable = false;
        canvas.overrideSorting = false;
    }
    
    public void Cast()
    {
        foreach (var item in activeTweens)
        {item.Kill();}
        CardManager.inst.Use(card,this);
      
    }

    public bool canCast()
    {return CardFunctions.canCast(card,true);}
    
    public void GoUp()
    {
        activeTweens.Add(rt.DOAnchorPosY(yUp,moveSpeed));
        up = true;
        canvas.sortingOrder = 99;
    }

    public void GoDown()
    {
        activeTweens.Add( rt.DOAnchorPos(ogPos,moveSpeed).OnComplete(()=> up = false)) ;
        canvas.sortingOrder = OGSorting;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!interactable)
        { return;}

        if(!up && !dragging)
        {GoUp();}
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if(!interactable)
        { return;}

        if(gameObject != null)
        {
            if(up && !dragging)
            {GoDown();}
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!interactable)
        { return;}

        if(eventData.button == PointerEventData.InputButton.Left)
        {dragging = true;}
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!interactable)
        { return;}

        if(gameObject != null)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                GoDown();
                dragging = false;
            }
        }    
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if(!interactable)
        { return;}

        if(eventData.button == PointerEventData.InputButton.Left)
        {rt.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;}
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!interactable || dragging)
        { return;}

        if(eventData.button == PointerEventData.InputButton.Right)
        {
            List<Card> cards = new List<Card>();
            cards.Add(card);
            CardViewer.inst.manuallyLoadCards = true;
            CardManager.inst.DeactivateHand();
            CardViewer.inst.ManuallyLoadCards(cards);
            CardViewer.inst.Open(card);
            GoDown();
        }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    { eventData.useDragThreshold = false; }
}
