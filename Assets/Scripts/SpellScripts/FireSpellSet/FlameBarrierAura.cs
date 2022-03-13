using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Flame Barrier", menuName = "ScriptableObjects/FireSpells/FlameBarrier", order = 1)]
public class FlameBarrierAura : baseAuraScript
{
    override public void onApply(Transform Player, Transform Target){
        PlayerStateScript p = Player.GetComponent<PlayerStateScript>();
        p.takeDamageMult = 0.5f;
    }

    override public void onTick(Transform Player, Transform Target, int stack, int tickNum){
        
    }

    override public void onExpire(Transform Player, Transform Target, int stack, int tickNum){
        PlayerStateScript p = Player.GetComponent<PlayerStateScript>();
        p.takeDamageMult = 1.0f;
    }   

    override public void onStack(Transform Player, Transform Target, int stack){
        
    }
}
