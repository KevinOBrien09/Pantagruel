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
    ExplorationAction cachedAction;
    
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

    public void MakeEncounterRoom(){
          currentRoomGraphic = Instantiate(dungeon.encounterRoom,levelHolder);
    }

    public void KillRoom(){
          Destroy(currentRoomGraphic );
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
            StartCoroutine(GoBackDelay());
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
            StartCoroutine(MoveDelay(action));
            break;
            default:
            Debug.LogAssertion("Default case");
            break;
        }
    }

    IEnumerator MoveDelay(ExplorationAction action)
    {
        if(action.validResponse.Contains("left")){
            MainCameraManager.inst.RotateX(-20);
        }
        else if(action.validResponse.Contains("right")){
                MainCameraManager.inst.RotateX(20);
        }
        yield return new WaitForSeconds(1);
        FadeViewportToBlack.inst.Fade(.5f);
        yield return new WaitForSeconds(.6f);
        if(Maths.PercentCalculator(action.encounterChanceWhenGoingToNextRoom))
        {
            GameMaster.inst.ChangeGameState(GameMaster.GameState.Battle);
            cachedAction = action;
           
        }
        else
        {
            ChangeRoom(action.nextRoom);
            MainCameraManager.inst.RotateX(0);
            yield return new WaitForSeconds(.25f);
            FadeViewportToBlack.inst.UnFade(1);
            yield return null;
        }
    }

    public void ContinueToRoom()
    {
        StartCoroutine(RealContinueToRoom());
    }

    public IEnumerator RealContinueToRoom()
    {
        FadeViewportToBlack.inst.Fade(.25f);
        yield return new WaitForSeconds(.25f);
        ChangeRoom(cachedAction.nextRoom);
        MainCameraManager.inst.RotateX(0);
        yield return new WaitForSeconds(.25f);
        FadeViewportToBlack.inst.UnFade(1);
        cachedAction = null;
        yield return null;
    }


    IEnumerator GoBackDelay()
    {
        
        MainCameraManager.inst.RotateX(-180);
        yield return new WaitForSeconds(1);
        FadeViewportToBlack.inst.Fade(.5f);
        yield return new WaitForSeconds(.6f);
        ChangeRoom(playerPath.Dequeue());
        MainCameraManager.inst.RotateX(0);
        yield return new WaitForSeconds(.25f);
        FadeViewportToBlack.inst.UnFade(1);
        yield return null;
    }
}
