using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodCurseAura", menuName = "ScriptableObjects/BloodSpells/BloodCurseAura", order = 1)]
public class BloodCurseAura : baseAuraScript
{
    override public void onApply(Transform Player, Transform Target)
    {
        Debug.Log("Blood Curse applied");
        PlayerStateScript pDmgMult = Target.GetComponent<PlayerStateScript>();
        pDmgMult.takeDamageMult += .25f;
    }

    override public void onTick(Transform Player, Transform Target, int stack, int tickNum)
    {

    }

    override public void onExpire(Transform Player, Transform Target, int stack, int tickNum)
    {
        Debug.Log("Blood Curse expired");
        PlayerStateScript pDmgMult = Target.GetComponent<PlayerStateScript>();
        pDmgMult.takeDamageMult -= .25f;
        if (stack >= 2)
        {
            pDmgMult.takeDamageMult -= .25f;
        }

    }

    override public void onStack(Transform Player, Transform Target, int stack)
    {
        Debug.Log("Blood Curse stacked");
        if(stack == 2)
        {
            PlayerStateScript pDmgMult = Target.GetComponent<PlayerStateScript>();
            pDmgMult.takeDamageMult += .25f;
        }

    }
}