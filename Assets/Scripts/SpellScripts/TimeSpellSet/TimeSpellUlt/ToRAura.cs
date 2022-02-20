using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToRAura", menuName = "ScriptableObjects/TimeSpells/ToRAura", order = 1)]
public class ToRAura : baseAuraScript
{
    override public void onApply(Transform Player, Transform Target)
    {
        Debug.Log("Time of Reckoning applied");
        PlayerStateScript pSpeed = Player.GetComponent<PlayerStateScript>();
        pSpeed.moveSpeed += 2.0f;
        pSpeed.castTimeMult -= .2f;
        pSpeed.manaCostMult -= .4f;
    }

    override public void onTick(Transform Player, Transform Target, int stack, int tickNum)
    {

    }

    override public void onExpire(Transform Player, Transform Target, int stack, int tickNum)
    {
        Debug.Log("Frostbite expired");
        PlayerStateScript pSpeed = Player.GetComponent<PlayerStateScript>();
        pSpeed.moveSpeed -= 2.0f;
        pSpeed.castTimeMult += .2f;
        pSpeed.manaCostMult += .4f;
    }

    override public void onStack(Transform Player, Transform Target, int stack)
    {

    }
}

