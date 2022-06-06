using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeOfReckoning", menuName = "ScriptableObjects/TimeSpells/TimeSpellUlt", order = 1)]
public class TimeOfReckoningSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast TimeOfReckoning");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot, int index)
    {
        PlayerStateScript caster = Player.GetComponent<PlayerStateScript>();
        caster.changeManaServerRpc(20.0f);
        caster.applyAura(Player, aura_A, 20);
        this.playAudio(Player, "onHit");
    }
}
