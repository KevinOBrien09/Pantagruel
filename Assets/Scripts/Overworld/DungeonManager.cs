using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DungeonManager : Singleton<DungeonManager>
{
    public Dungeon dungeon;
    public DungeonRoom currentRoom;
    [SerializeField] TMP_Typewriter promptText;
    [SerializeField] TextMeshPro wwyd;
    [SerializeField] Transform levelHolder;
    public GameObject currentRoomGraphic;
    Queue<DungeonRoom> playerPath = new Queue<DungeonRoom>();
    public int layer = 0;
    
    void Start()
    {
       ChangeRoom(dungeon.startingRoom);
    }

    public void ChangeRoom(DungeonRoom r)
    {
        currentRoom = r;
        foreach (var item in currentRoom.options)
        {item.ForceToLower();}
        promptText.Play(">" + r.prompt.ToLower(),21,()=>{});
  
        layer++;
        if(currentRoom != null)
        {
            Destroy(currentRoomGraphic );
            if(r.roomPrefab != null){
            currentRoomGraphic = Instantiate(r.roomPrefab,levelHolder);
            }
         
        }
    }
    public (string,bool) CheckEverPresent(string input)
    {
        if(input == "back"|input == "go back"|input == "previous"|input == "leave"|input == "leave room")
        {
            if(layer!= 1)
            { 
                GoBack();
                return ("went back",true);
            }
            else
            {return (string.Empty,true);}
        }
        else
        {return ("",false);}
    }
    
    public void GoBack()
    {
        if(layer !=1)
        {
            layer = layer -2;
            ChangeRoom(playerPath.Dequeue());
        }
        else
        {Debug.Log("Leave dungeon?");}
    }

    public void ParseAction(ExplorationAction action)
    {
        switch (action.result)
        {
            case ExplorationResult.GoToRoom:
            playerPath.Enqueue(currentRoom);
            ChangeRoom(action.nextRoom);
            break;
            default:
            Debug.LogAssertion("Default case");
            break;
        }
    }
}
