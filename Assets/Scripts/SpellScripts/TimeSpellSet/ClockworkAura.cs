using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Clockwork", menuName = "ScriptableObjects/TimeSpells/Clockwork", order = 1)]
public class ClockworkAura : baseAuraScript
{
    override public void onApply(Transform Player, Transform Target)
    {
        Debug.Log("Clockwork applied");
        PlayerStateScript pSpeed = Player.GetComponent<PlayerStateScript>();
        pSpeed.changeSpeed(2.0f);
    }

    override public void onTick(Transform Player, Transform Target, int stack, int tickNum)
    {

    }

    override public void onExpire(Transform Player, Transform Target, int stack, int tickNum)
    {
        Debug.Log("Frostbite expired");
        PlayerStateScript pSpeed = Player.GetComponent<PlayerStateScript>();
        if (stack == 2){
            pSpeed.changeSpeed(-4.0f);
        } else {
            pSpeed.changeSpeed(-2.0f);
        }
    }

    override public void onStack(Transform Player, Transform Target, int stack)
    {
        Debug.Log("Clockwork stacked");
        if (stack == 2)
        {
            PlayerStateScript pSpeed = Target.GetComponent<PlayerStateScript>();
            pSpeed.changeSpeed(2.0f);
        }

    }
}
