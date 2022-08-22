using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHandler : MonoBehaviour
{
    public List<SkillHolder> skillHolders = new List<SkillHolder>();
    [SerializeField] GameObject cannotCastButton;

    public void InitNewSkills(BeastData b)
    {
        for (int i = 0; i < b.skills.Count; i++)
        {skillHolders[i].Apply(b.skills[i]);}
    }

    public void DeActivateSkillButtons()
    {
        foreach (var item in skillHolders)
        {
            item.button.interactable = false;
            item.button.showHover = false;
            item.button.BlackWhite(true);
        }
    }

    public void ReActivateSkillButtons(Beast b)
    {
        if(b.currentSP >= CheckSkillCost.MinCost(b)){
            cannotCastButton.SetActive(false);
           // Debug.Log("Can Cast");
        }
        else
        {
            cannotCastButton.SetActive(true);
            Debug.Log("Cannot cast");
        }


        foreach (SkillHolder item in skillHolders)
        {
            if(item.data.cost.resource == ResourceCurrency.SP )
            {
                if(b.currentSP >= item.data.cost.amount )
                {
                    if(item.data.cantCastWithFullHealth)
                    {
                        if(BattleManager.inst.activePlayerBeast.currentHealth == BattleManager.inst.activePlayerBeast.data.stats.maxHealth)
                        {
                            item.button.interactable = false;
                            item.button.showHover = true;
                            item.button.BlackWhite(true);
                            return;
                        }

                    }
                    item.button.interactable = true;
                     item.button.showHover = true;
                    item.button.BlackWhite(false);
                }
                else
                {
                    item.button.interactable = false;
                    item.button.showHover = true;
                    item.button.BlackWhite(true);
                }
            }
            else if(item.data.cost.resource == ResourceCurrency.HP )
            {
                if(b.currentHealth > item.data.cost.amount )
                {
                    item.button.interactable = true;
                     item.button.showHover = true;
                    item.button.BlackWhite(false);
                }
                else
                {
                    item.button.interactable = false;
                     item.button.showHover = true;
                    item.button.BlackWhite(true);
                }

            }
            
            
           
        }

    }

}