using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "IceSpellE", menuName = "ScriptableObjects/IceSpells/IceSpellE", order = 1)]
public class SnowballSpell : baseSpellScript
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
        float duration = 8.0f;
        if (target.hasAura(aura_A.id) == -1)
        {
                target.applyAura(Player, aura_A, duration); 
        }else
        {
            target.takeDamage(damage);
        }

        Debug.Log("Hit Snowball Spell");

        FindObjectOfType<SpellRpcs>().spawnHitParticleServerRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, index, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, true);
    }
}
