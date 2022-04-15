using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FrostBite", menuName = "ScriptableObjects/IceSpells/Frostbite", order = 1)]
public class FrostbiteAura : baseAuraScript
{
    override public void onApply(Transform Player, Transform Target)
    {
        Debug.Log("Frostbite applied");
        PlayerStateScript pSpeed = Target.GetComponent<PlayerStateScript>();
        pSpeed.moveSpeed -= 2.0f;
    }

    override public void onTick(Transform Player, Transform Target, int stack, int tickNum)
    {

    }

    override public void onExpire(Transform Player, Transform Target, int stack, int tickNum)
    {
        Debug.Log("Frostbite expired");
        PlayerStateScript pDmgMult = Target.GetComponent<PlayerStateScript>();
        pDmgMult.moveSpeed += 2.0f;
        if (stack >= 3)
        {
            pDmgMult.takeDamageMult -= .20f;
        }
        if (stack >= 2)
        {
            pDmgMult.castTimeMult -= .20f;
        }
    }

    override public void onStack(Transform Player, Transform Target, int stack)
    {
        Debug.Log("Frostbite stacked");
        if(stack == 2)
        {
            PlayerStateScript pCastMult = Target.GetComponent<PlayerStateScript>();
            pCastMult.castTimeMult += .20f;
        }
        if(stack == 3)
        {
            PlayerStateScript pDmgMult = Target.GetComponent<PlayerStateScript>();
            pDmgMult.takeDamageMult += .20f;

        }
    }
}