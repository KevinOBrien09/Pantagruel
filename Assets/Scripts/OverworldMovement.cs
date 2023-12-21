using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class PlayerSaveData{
    public Vector3 pos;
    public Vector3 rot;
    public CardinalDirection lastRot;
   
    public bool torchState;
    public float torchTimer;
}

public class OverworldMovement : MonoBehaviour
{
    public static bool canMove;
    public bool isMoving;
    public RotatePlayer rotate;
    [SerializeField] Camera cam;
    [SerializeField] LayerMask mask;
    [SerializeField] float moveSpeed;
    [SerializeField] float stride;
    [SerializeField] AudioSource audioSource;
    public bool detectEncouters;
    [SerializeField] float rayLength = 3;
    [SerializeField] Torch torch;
    public int encounterTimeOut;
    public int encounterChance = 10;
    public int timeOutMin,timeOutMax;
    bool lastTorchState;
    float ogPOV;
    public float zoomPOV;
    
   
    void Start(){
        ogPOV = cam.fieldOfView;
    }
    void OnEnable()
    {canMove = true;}
    
    void Update()
    {   if(canMove)
        {
        if(InputManager.input.wasdDown[0])
        {StartMove(Dir.Forwards);}
        else if(InputManager.input.wasdDown[1])
        {StartMove(Dir.Left);}
        else if(InputManager.input.wasdDown[2])
        {StartMove(Dir.Backwards);}
        else if(InputManager.input.wasdDown[3])
        {StartMove(Dir.Right); }

        if(InputManager.input.rotateLeft){
            rotate.StartRotate(Dir.Left);
        }
        else if(InputManager.input.rotateRight){
            rotate.StartRotate(Dir.Right);
        }
        }
    }

    public void StartMove(Dir dir)
    {
        
        if(!rotate.isRotating & !isMoving)
        {Move(dir);}
        
    }

    public float ZOOMPOV(){
        float dur = .2f;
        cam.DOFieldOfView(zoomPOV,dur);
        return dur;
    }

    public void ResetPOV(){
    cam.DOFieldOfView(ogPOV,0);
    }

    public void ResetPOVTimer(){
    cam.DOFieldOfView(ogPOV,.2f);
    }

    public void ChangeFootStepSFX(AudioClip clip){
        if(clip == null){
            return;
        }
        audioSource.clip = clip;
    }
    
    void Move(Dir dir)
    {
       
        if(CheckWall(dir))
        {
            Debug.Log("Wall In Front");
            return;
        }
        else
        {
            torch.Bounce();
            torch.DeductStep();
            EventManager.inst.onStep.Invoke();
            audioSource.pitch = Random.Range(.9f,1.1f);
            audioSource.PlayOneShot(audioSource.clip);
            Vector3 move = Vector3.zero;
            isMoving = true;
            if(encounterTimeOut != 0)
            {
                encounterTimeOut--;
            }
            
            switch(rotate.currentRot)
            {
                case CardinalDirection.N:
                    switch (dir)
                    {
                        case Dir.Forwards:
                        transform.DOMoveZ(transform.position.z + stride,moveSpeed).OnComplete(() => EndStep());
                        break;
                        case Dir.Backwards:
                        transform.DOMoveZ(transform.position.z + -stride,moveSpeed).OnComplete(() =>  EndStep());
                        break;
                        case Dir.Left:
                        transform.DOMoveX(transform.position.x -stride,moveSpeed).OnComplete(() => EndStep());
                        break;
                        case Dir.Right:
                        transform.DOMoveX(transform.position.x + stride,moveSpeed).OnComplete(() => EndStep());
                        break;
                    }
                break;

                case CardinalDirection.S:
                    switch (dir)
                    {
                        case Dir.Forwards:
                        transform.DOMoveZ(transform.position.z + -stride,moveSpeed).OnComplete(() => EndStep());
                        break;
                        case Dir.Backwards:
                        transform.DOMoveZ(transform.position.z + stride,moveSpeed).OnComplete(() => EndStep());
                        break;
                        case Dir.Left:
                        transform.DOMoveX(transform.position.x + stride,moveSpeed).OnComplete(() => EndStep()); 
                        break;
                        case Dir.Right:
                        transform.DOMoveX(transform.position.x -stride,moveSpeed).OnComplete(() =>  EndStep());
                        break;
                    }
                break;

                case CardinalDirection.E:
                    switch (dir)
                    {
                        case Dir.Forwards:
                        transform.DOMoveX(transform.position.x + stride,moveSpeed).OnComplete(() =>  EndStep());
                        break;
                        case Dir.Backwards:
                        transform.DOMoveX(transform.position.x -stride,moveSpeed).OnComplete(() =>  EndStep());
                        break;
                        case Dir.Left:
                        transform.DOMoveZ(transform.position.z + stride,moveSpeed).OnComplete(() => EndStep());
                        break;
                        case Dir.Right:
                        transform.DOMoveZ(transform.position.z + -stride,moveSpeed).OnComplete(() =>  EndStep());
                        break;
                    }
                break;

                case CardinalDirection.W:
                    switch (dir)
                    {
                        case Dir.Forwards:
                        transform.DOMoveX(transform.position.x -stride,moveSpeed).OnComplete(() =>  EndStep());
                        break;
                        case Dir.Backwards:
                        transform.DOMoveX(transform.position.x + stride,moveSpeed).OnComplete(() =>   EndStep());
                        break;
                        case Dir.Left:
                        transform.DOMoveZ(transform.position.z + -stride,moveSpeed).OnComplete(() =>  EndStep());
                        break;
                        case Dir.Right:
                        transform.DOMoveZ(transform.position.z + stride,moveSpeed).OnComplete(() => EndStep() );
                        break;
                    }
                break;
            }
        }
    }

    void CheckForEncounter()
    {
        if(detectEncouters){
            if(encounterTimeOut == 0)
            {
                int n = Random.Range(0,100);
                if(n <= encounterChance)
                {
                //  Debug.Log( n + "Encounter");
                    encounterTimeOut = Random.Range(timeOutMin,timeOutMax);
                    canMove = false;
                    lastTorchState = torch.torchOn;
                    torch.DisableTorch();
                    BattleManager.inst.StartBattle(BattleType.Wild);
                }
            }
        }
       
    }

    public void ReactivateMove()
    {
        canMove = true;
        if(lastTorchState)
        {torch.EnableTorch();}
    }

    public void EndStep()
    {
        if(!torch.torchOn)
        {CheckForEncounter();}
          
        isMoving = false;
    }
    
    public bool CheckWall(Dir dir)
    {
        Vector3 moveDirection = Vector3.zero;
        switch (dir)
        {
            case Dir.Forwards:
            moveDirection = transform.forward;
            break;
            case Dir.Backwards:
            moveDirection = -transform.forward;
            break;
            case Dir.Left:
            moveDirection = -transform.right;
            break;
            case Dir.Right:
            moveDirection = transform.right;
            break;
        }

        bool hit = Physics.Raycast(cam.transform.position,moveDirection,rayLength,layerMask:mask);
        if(hit)
        {return true;}
        else
        {return false;}
    }



    public PlayerSaveData Save()
    {
        PlayerSaveData saveData = new PlayerSaveData();
        saveData.pos = transform.position;
        saveData.rot = transform.rotation.eulerAngles;
        saveData.lastRot = rotate.currentRot;
        saveData.torchState = torch.torchOn;
        saveData.torchTimer = torch.currentSteps;
        return saveData;
    }

    public void InitPosRot(Vector3 pos,Vector3 rot)
    {
        transform.position = pos;
        transform.rotation = Quaternion.Euler(rot.x,rot.y,rot.z);
    }

    public void Load(PlayerSaveData data)
    {
        InitPosRot(data.pos,data.rot);
        rotate.InitRotation(data.lastRot);
        torch.Load(data.torchState,data.torchTimer);
    }
}