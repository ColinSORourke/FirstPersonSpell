using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "Tick", menuName = "ScriptableObjects/TimeSpells/TimeSpellA", order = 1)]
public class BasicTimeSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast Time Spell A");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot, int index)
    {
        PlayerStateScript tHP = Target.GetComponent<PlayerStateScript>();
        tHP.takeDamage(damage);

        PlayerStateScript caster = Player.GetComponent<PlayerStateScript>();
        caster.changeManaServerRpc(10.0f);

        FindObjectOfType<SpellRpcs>().spawnHitParticleServerRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, index, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, true);
        this.playAudio(Target, "onHit");
    }
}
