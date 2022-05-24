using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "IceSpellG", menuName = "ScriptableObjects/IceSpells/IceSpellG", order = 1)]
public class IceRamperSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast Snowball Spell");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot, int index)
    {
        PlayerStateScript target = Target.GetComponent<PlayerStateScript>();
        target.takeDamage(damage);
        if (target.hasAura(aura_A.id) != -1)
        {
            PlayerStateScript player = Player.GetComponent<PlayerStateScript>();
            player.changeUltServerRpc(1.0f);
        }
        FindObjectOfType<SpellRpcs>().spawnHitParticleServerRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, index, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, true);

        Debug.Log("Hit Snowball Spell");
    }
}

