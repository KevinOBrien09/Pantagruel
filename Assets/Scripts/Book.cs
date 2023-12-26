// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using System.Linq;
// using TMPro ;

// [System.Serializable]
// public class BestiaryData
// {
//     public int ID;
//     public bool met;

//     public BestiaryData (int id = 0,bool m = false)
//     {
//         ID = id;
//         met = m;

//     }
// }

// public class Book : MonoBehaviour
// {
//     public const int firstPage = 0;
//     public int currentPage;
//     public int totalPages;
//     [SerializeField] List<BestiaryData> knownBeasts = new List<BestiaryData>();
//     [SerializeField] AudioSource pageFlick;
//     public List<BeastScriptableObject> beastScriptableObjects = new List<BeastScriptableObject>();
//     [SerializeField] Image picture;
//     [SerializeField] TextMeshProUGUI nameText;
//     [SerializeField] TextMeshProUGUI idText;
//     [SerializeField] TextMeshProUGUI mainFamilyText;
//     [SerializeField] TextMeshProUGUI secondaryFamilyText;
//     [SerializeField] TextMeshProUGUI flavourText;
//     [SerializeField] Color32 hiddenColour;
//     public enum BeastBookInit{KnowAll,KnowNone,KnowRandom}
//    public BeastBookInit startState;
//    public BeastEditor beastEditor;
//     Dictionary<int,bool> id = new Dictionary<int, bool>();
    
//     void Start()
//     {   
//         switch(startState)
//         {
//             case BeastBookInit.KnowAll:
//             for (int i = 0; i < beastScriptableObjects.Count+1; i++)
//             {knownBeasts.Add(new BestiaryData(id:i,true));}
//             break;
//             case BeastBookInit.KnowNone:
//              for (int i = 0; i < beastScriptableObjects.Count+1; i++)
//             {knownBeasts.Add(new BestiaryData(id:i,false));}
//             break;
//             case BeastBookInit.KnowRandom:

//                 for (int i = 0; i < beastScriptableObjects.Count+1; i++)
//                 {
//                     int q = Random.Range(0,2);
//                     if(q == 0)
//                     {
//                         knownBeasts.Add(new BestiaryData(id:i,true));
//                     }
//                     else
//                     {
//                         knownBeasts.Add(new BestiaryData(id:i,false));
//                     }
                  
//                 }
//             break;
//         }
    
    
       
//         for (int i = 0; i < knownBeasts.Count; i++)
//         {id.Add(knownBeasts[i].ID,knownBeasts[i].met);}

//         beastScriptableObjects = 
//         new List<BeastScriptableObject>(beastScriptableObjects.OrderBy(f => f.beastData.bestiaryID));

//         totalPages = beastScriptableObjects.Count;
//         AssignInfo(beastScriptableObjects[0].beastData);
//         StartCoroutine(s());
//         IEnumerator s()
//         {
//             yield return new WaitForEndOfFrame();
//             if(beastEditor!=null){
//             beastEditor.ChangeBeast(beastScriptableObjects[currentPage]);}
//         }
    
//     }
 

//     void Update()
//     {
//         // if(Input.GetKeyDown(KeyCode.A))
//         // {MovePageLeft();}

//         // if(Input.GetKeyDown(KeyCode.D))
//         // {MovePageRight();}
//     }
    
//     void MovePageRight()
//     {
//         currentPage++;
//         currentPage = Mathf.Clamp(currentPage,0,totalPages);
//         if(beastScriptableObjects.ElementAtOrDefault(currentPage) != null)
//         {
//             AssignInfo(beastScriptableObjects[currentPage].beastData);
//             SFX(); 
//         }
//         EventSystem.current.SetSelectedGameObject(null);

//         if(beastEditor!=null){
//             beastEditor.ChangeBeast(beastScriptableObjects[currentPage]);
//            }
     
//     }

//     void SFX()
//     {
//         pageFlick.pitch = Random.Range(.8f,1f);
//         pageFlick.PlayOneShot(pageFlick.clip);
//     }

//     void MovePageLeft()
//     {
//         currentPage--;
//         currentPage = Mathf.Clamp(currentPage,0,totalPages);
       
//         if(beastScriptableObjects.ElementAtOrDefault(currentPage)!= null)
//         {

//             AssignInfo(beastScriptableObjects[currentPage].beastData);
//             SFX();
//         }
//         EventSystem.current.SetSelectedGameObject(null);
           
//            if(beastEditor!=null){
//             beastEditor.ChangeBeast(beastScriptableObjects[currentPage]);
//            }
     
//     }

//     public void AssignInfo(BeastData beastData)
//     {
    
//         if(id[beastData.bestiaryID])
//         {
//             picture.sprite = beastData.mainSprite;
//             picture.color = Color.white;
//             nameText.text = beastData.beastName;
//             idText.text =  RomanNumerals.ToRoman(beastData.bestiaryID);
//             flavourText.text = beastData.flavourText;

//             // if(beastData.mainFamily != Family.NA)
//             // {
//             //     mainFamilyText.gameObject.SetActive(true);
//             //     mainFamilyText.text = beastData.mainFamily.ToString();
//             // }
//             // else{mainFamilyText.gameObject.SetActive(false);}

//             // if(beastData.secondaryFamily != Family.NA)
//             // {
//             //     secondaryFamilyText.gameObject.SetActive(true);
//             //    secondaryFamilyText.text = beastData.secondaryFamily.ToString();
//             // }
//             // else{secondaryFamilyText.gameObject.SetActive(false);}


          
//         }
//         else
//         {
//             picture.sprite = beastData.mainSprite;
//             picture.color = hiddenColour;
//             nameText.text = "???";
//             idText.text =  RomanNumerals.ToRoman(beastData.bestiaryID);
//             flavourText.text = "???";
//         }
      
//     }
    
// }
