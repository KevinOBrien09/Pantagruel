using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Effects/MultiTargetDamage", fileName = "MTDamage")]
public class MultiTargetDamage :Effect
{
    public List<TargetDamage> targetDamages = new List<TargetDamage>();
    public override void Use(EffectArgs args)
    {
        foreach (var item in targetDamages)
        {
            switch (item.target)
            {
                case WhatTarget.ENEMYBEAST:
                if(!args.isPlayer)
                {
                    PlayerParty.inst.activeBeast.TakeDamage(item.damageValue);
                }
                else{
                       RivalBeastManager.inst.activeBeast.TakeDamage(item.damageValue);
                }
             
                break;
                case WhatTarget.ENEMYPET:

                if(!args.isPlayer)
                {
                   if(PetManager.inst.playerPet != null)
                    {
                        PetManager.inst.playerPet.TakeDamage(item.damageValue);
                    }   
                }
                else{
                    if(PetManager.inst.enemyPet != null)
                    {
                        PetManager.inst.enemyPet.TakeDamage(item.damageValue);
                    } 
                }
                
               
                break;
                case WhatTarget.PLAYERBEAST:
                  if(!args.isPlayer)
                    {
                           RivalBeastManager.inst.activeBeast.TakeDamage(item.damageValue);
                      
                    }
                    else{
                       PlayerParty.inst.activeBeast.TakeDamage(item.damageValue);
                    }

                break;
                case WhatTarget.PLAYERPET:
                     if(args.isPlayer)
                {
                   if(PetManager.inst.playerPet != null)
                    {
                        PetManager.inst.playerPet.TakeDamage(item.damageValue);
                    }   
                }
                else{
                    if(PetManager.inst.enemyPet != null)
                    {
                        PetManager.inst.enemyPet.TakeDamage(item.damageValue);
                    } 
                }
                
                break;
                
                
            }
            
        }

       
    }

}
public enum WhatTarget{ENEMYBEAST,ENEMYPET,PLAYERPET,PLAYERBEAST}
[System.Serializable]
public class TargetDamage{
    public WhatTarget target;
    public int damageValue;

}