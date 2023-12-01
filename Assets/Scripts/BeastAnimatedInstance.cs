using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EasySpringBone;
using System.Linq;

public class BeastAnimatedInstance : MonoBehaviour
{
    public List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    public List<SpringBone> springBones = new List<SpringBone>();
    public Color32 grey = new Color32(97,97,97,255);
    Color defaultColour;
    bool dodgeDir;
    Animator animator;
    Entity beast;
    StatusEffectHandler effectHandler;
    public ParticleSystem blood;
    public GameObject stunBirds;
    public void Init(Entity b)
    {
        SpriteRenderer a = null;
       
        if(gameObject.TryGetComponent<SpriteRenderer>(out a))
        {renderers.Add(a);}
        
        springBones = gameObject.GetComponentsInChildren<SpringBone>().ToList();
        renderers = gameObject.GetComponentsInChildren<SpriteRenderer>().ToList();
        defaultColour = Color.white;
        if(gameObject.transform.Find("Blood") != null){
        blood = gameObject.transform.Find("Blood").GetComponent<ParticleSystem>();
        blood.gameObject.layer = this.gameObject.layer;
        }
        
        Animator an;
        if(TryGetComponent<Animator>(out an))
        {animator = an;}
        if(b.GetType() == typeof(Beast)){
            effectHandler = Instantiate(BattleManager.inst.statusEffectHandlerPrefab,transform);
            effectHandler.Init((Beast) b,this.gameObject.layer);
        }
        else{
            Debug.Log("NoPetStatusEffects");
        }
        dodgeDir = Maths.FiftyFifty();
        b.animatedInstance = this;
        beast = b;
    }

    public void Bleed(){
        if(blood!= null){
            blood.Play();
           
        }
        //  AudioManager.inst.GetSoundEffect().Play(SystemSFX.inst.bleed);
    }

    public void Stun(){
        TakeDamage(Color.yellow);
        stunBirds.SetActive(true);
     
      
    }

     public void RemoveStun(){
        
        stunBirds.SetActive(false);
        
   

      
    }

    public void Dodge()
    {

       
        AudioManager.inst.GetSoundEffect().Play(BattleManager.inst.dodgeSFX[Random.Range(0,BattleManager.inst.dodgeSFX.Count)]);
        ToggleSpringBones(false);
        if(dodgeDir)//right
        {
            SpawnText();
            transform.DOMoveX(1f,.2f).OnComplete(()=>
            {
                transform.DOMoveX(-.2f,.2f).OnComplete(()=>
                {transform.DOMoveX(0,.2f);ToggleSpringBones(true);});
            });
        }
        else//left
        {
            transform.DOMoveX(-1f,.2f).OnComplete(()=>
            {SpawnText();
                transform.DOMoveX(.2f,.2f).OnComplete(()=>
                {transform.DOMoveX(0,.2f);ToggleSpringBones(true);});
            });
        }

        if(dodgeDir){
dodgeDir = false;
        }else{
            dodgeDir = true;
        }
        BattleTicker.inst.Type("Dodge!");
        void SpawnText(){
            // BattleTextBehaviour b = Instantiate(BattleManager.inst.battleTextBehaviourPrefab,transform);
            // b. transform.SetParent(null,true);
            // b.transform.position = new Vector3(0,0,2);
        
            // b.gameObject.layer = this.gameObject.layer;
            // b.Spawn("Dodge!",Color.white,beast.goLeft);
        }
      
    }

    public void TakeDamage(Color color)
    {
        Vector3 p = transform.position;

        ToggleSpringBones(false);
        if(animator != null){
        animator.speed = 0;
        }
   
      
        transform.DOShakePosition(.5f,new Vector3(.1f,.1f,0) ,10,90,false,true).OnComplete(()=>
        {
            transform.DOMove(p,.1f);
            ToggleSpringBones(true);
        });
    
        

        foreach (var item in renderers)
        {
            StartCoroutine(q());
            IEnumerator q(){
                item.DOColor(color,0);
                yield return new WaitForSeconds(.5f);
                item.DOColor(defaultColour,.2f);
                if(animator != null){
                animator.speed = 1;
                }
            }
        }
    }

   public void ToggleSpringBones(bool state){
  foreach (var item in springBones)
            {item.ignoreSpringBone = true;}
    }

    public void Heal()
    {
        foreach (var item in renderers)
        {
            item.DOColor(Color.green,.3f).OnComplete(()=>{
            item.DOColor(defaultColour,.3f);
            });
        }
    }

    public float MoveBack(EntityOwnership ownership){
        
        float dur = .7f;
        ToggleSpringBones(false);

        float a = 0;
        float b = 0;
        float c =0;
        if(ownership == EntityOwnership.ENEMY){
            a = 1.5f;
            b = 4.5f;
            c = 4f;
        }
        else{
              a = .85f;
            b = 2.5f;
            c = 2f;
        }
        transform.DOMoveZ(a,.2f).OnComplete(()=>
        {
            transform.DOMoveZ(b,.3f).OnComplete(()=>
            {
               transform.DOMoveZ(c,.2f).OnComplete(()=>
                {
                    ToggleSpringBones(true);
                
                });
            
            });
          
        });
        
        defaultColour = grey;
        foreach (var item in renderers)
        {
            item.DOColor(defaultColour,.3f);
        }
        if(ownership == EntityOwnership.ENEMY){
            BattleField.inst.enemyBeast.Move( BattleField.inst.yPositions[0]);
        }
        else{
            BattleField.inst.playerBeast.Move( BattleField.inst.yPositions[3]);
        }
        
      return dur;
    }

    public void MoveForward(EntityOwnership ownership)
    {
        float a = 0;
        float b = 0;
        float c =0;
        if(ownership == EntityOwnership.ENEMY)
        {
            a = 4.5f;
            b = 1.85f;
            c = 2f;
        }
        else
        {
            a = 2.5f;
            b = .85f;
            c = 1f;
        }

        ToggleSpringBones(false);
        transform.DOMoveZ(a,.2f).OnComplete(()=>
        {   
            transform.DOMoveZ(b,.3f).OnComplete(()=>
            { 
                transform.DOMoveZ(c,.2f).OnComplete(()=>
                { 
                    ToggleSpringBones(true);
                });
            });
            
        });

   
              
        defaultColour = Color.white;
        foreach (var item in renderers)
        {item.DOColor(defaultColour,.3f);}
           if(ownership == EntityOwnership.ENEMY){
        BattleField.inst.enemyBeast.Move(  BattleField.inst.yPositions[1] );
           }
           else{
             BattleField.inst.playerBeast.Move( BattleField.inst.yPositions[2]);
           }
       
    }

    public void Dissolve(float dissTime)
    {
       // float dissTime = 1;
        foreach (var item in renderers)
        {
            DOVirtual.Float( item.material.GetFloat("_DissolvePower"),0,dissTime,v  => 
            { item.material.SetFloat("_DissolvePower",v);});
        }
       
        StartCoroutine(q());
        IEnumerator q()
        {
            yield return new WaitForSeconds(dissTime);
            foreach (var item in renderers)
            {
                item.material.SetFloat("_EmissionThickness",0);
            }
        }
    }

}