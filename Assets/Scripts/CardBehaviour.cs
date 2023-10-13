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
    public TextMeshProUGUI skillDesc;  
    public GameObject manaGem;
    public CanvasGroup canvasGroup;
    public Image icon;
    public Image BG;
    public List<Image> imagesToHaveWobbleShader = new List<Image>();
    public List<Image> imagesToHaveDissolveShader = new List<Image>();
    public Material vaporousWobble,vaporousDissolve;
    public float yUp;
    public bool interactable;
    public bool isVapour;
    Canvas parentCanvas;
    Vector2 ogPos;
    List<Tween> activeTweens = new List<Tween>();
    string desc;
    float moveSpeed = .2f;
    int OGSorting;
    bool up;
    bool descShown;
    
    void Start()
    {
        OGSorting = canvas.sortingOrder;
        parentCanvas = GameObject.Find("UICanvas").GetComponent<Canvas>();
        ogPos = rt.anchoredPosition;
    }

    public void Init(Card newCard,bool isVapour_ = false)
    {
        card = newCard;
        icon.sprite = card.picture;
        cardName.text = card.cardName;
        desc = newCard.desc;
        if(card.manaCost == 0){
            manaCost.text = "O";
        }
        else{
            manaCost.text = RomanNumerals.ToRoman(card.manaCost);
        }

        if(isVapour_)
        {
            isVapour = true;
            BG.color = Color.white;
            skillDesc.color = Color.black;
            cardName.text = "<i>Vaporous </i>" + card.cardName;
           // desc = "<size=80%><i>RAPID: Will be DESTROYED at the end of this turn.</i> <br></size>" + desc;
            foreach (var item in imagesToHaveWobbleShader)
            {
                item.material = vaporousWobble;
            }
          
        }
       
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

        // cardName.transform.SetParent(BG.transform);
        // manaCost.transform.SetParent(BG.transform);
      
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
    {
        interactable = false;
        canvas.overrideSorting = false;
    }
    
    public void Cast()
    {
        foreach (var item in activeTweens)
        {item.Kill();}
        CardManager.inst.Use(card,this);
        Debug.Log(card.cardName + " was Cast");
    }

    public bool canCast()
    {return CardManager.inst.canCast(card);}

    public void ShowDesc()
    {
        descShown = true;
        icon.enabled = false;
     //   manaCost.enabled = false;
        skillDesc.enabled = true;
        skillDesc.text = desc;
        manaGem.SetActive(false);
    }
    
    public void HideDesc()
    {
        descShown = false;
        icon.enabled = true;
        //manaCost.enabled = true;
          manaGem.SetActive(true);
        skillDesc.enabled = false;
        skillDesc.text = string.Empty;
    }

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
            {
                if(descShown)
                {HideDesc();}
                GoDown();
            }
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
                if(descShown)
                {HideDesc();}
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
        if(!interactable)
        { return;}

        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(descShown)
            {HideDesc();}
            else
            {ShowDesc();}
        }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    { eventData.useDragThreshold = false; }
}
