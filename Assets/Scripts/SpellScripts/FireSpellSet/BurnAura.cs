using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Burn", menuName = "ScriptableObjects/FireSpells/Burn", order = 1)]
public class BurnAura : baseAuraScript
{
    override public void onApply(Transform Player, Transform Target){
        Debug.Log("Burn applied");
    }

    override public void onTick(Transform Player, Transform Target, int stack, int tickNum){
        PlayerStateScript tHP = Target.GetComponent<PlayerStateScript>();
        tHP.takeDamage(damage, false);
        Debug.Log("Burn ticked");
    }

    override public void onExpire(Transform Player, Transform Target, int stack, int tickNum){
        Debug.Log("Burn expired");
    }   

    override public void onStack(Transform Player, Transform Target, int stack){
        Debug.Log("Burn stacked");
    }
}
