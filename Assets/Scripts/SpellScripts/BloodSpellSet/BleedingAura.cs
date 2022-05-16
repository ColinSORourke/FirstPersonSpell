using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bleeding", menuName = "ScriptableObjects/BloodSpells/Bleeding", order = 1)]
public class BleedingAura : baseAuraScript
{
    override public void onApply(Transform Player, Transform Target)
    {
        Debug.Log("Bleeding applied");
    }

    override public void onTick(Transform Player, Transform Target, int stack, int tickNum)
    {
        PlayerStateScript tHP = Target.GetComponent<PlayerStateScript>();
        tHP.takeDamage(stack, false);
        Debug.Log("Bleeding ticked");
    }

    override public void onExpire(Transform Player, Transform Target, int stack, int tickNum)
    {
        Debug.Log("Bleeding expired");
        if(stack == 3)
        {
            PlayerStateScript pManaRegen = Target.GetComponent<PlayerStateScript>();
            pManaRegen.doManaRegen = true;

        }
    }

    override public void onStack(Transform Player, Transform Target, int stack)
    {
        Debug.Log("Bleeding stacked");

        if(stack == 3)
        {
            PlayerStateScript pManaRegen = Target.GetComponent<PlayerStateScript>();
            pManaRegen.doManaRegen = false;

        }
    }
}