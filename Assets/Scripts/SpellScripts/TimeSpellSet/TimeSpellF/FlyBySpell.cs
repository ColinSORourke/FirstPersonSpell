using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "FlyBy", menuName = "ScriptableObjects/TimeSpells/TimeSpellF", order = 1)]
public class FlyBySpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast FlyBy");
        PlayerStateScript caster = Player.GetComponent<PlayerStateScript>();

        Debug.Log(caster.moveSpeed);
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot, int index)
    {
        PlayerStateScript caster = Player.GetComponent<PlayerStateScript>();

        PlayerStateScript tHP = Target.GetComponent<PlayerStateScript>();
        tHP.takeDamage(damage + (caster.moveSpeed/2));

        FindObjectOfType<SpellRpcs>().spawnHitParticleServerRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, index, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, true);
        this.playAudio(Target, "onHit");
    }
}
