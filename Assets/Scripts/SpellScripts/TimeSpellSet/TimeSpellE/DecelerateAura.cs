using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecelerationAura", menuName = "ScriptableObjects/TimeSpells/DecelerateAura", order = 1)]
public class DecelerateAura : baseAuraScript
{
    override public void onApply(Transform Player, Transform Target)
    {
        Debug.Log("Deceleration applied");
        PlayerStateScript tSpeed = Target.GetComponent<PlayerStateScript>();
        //tSpeed.moveSpeed -= 2.0f;
        tSpeed.changePlayerMovementSpeedRpc(tSpeed.moveSpeed - 2.0f);
    }

    override public void onTick(Transform Player, Transform Target, int stack, int tickNum)
    {

    }

    override public void onExpire(Transform Player, Transform Target, int stack, int tickNum)
    {
        Debug.Log("Deceleration expired");
        PlayerStateScript tSpeed = Target.GetComponent<PlayerStateScript>();
        //tSpeed.moveSpeed += 2.0f;
        tSpeed.changePlayerMovementSpeedRpc(tSpeed.moveSpeed + 2.0f);
    }

    override public void onStack(Transform Player, Transform Target, int stack)
    {

        

    }
}
