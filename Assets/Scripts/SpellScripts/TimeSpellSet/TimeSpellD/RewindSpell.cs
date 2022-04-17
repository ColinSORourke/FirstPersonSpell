using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "Rewind", menuName = "ScriptableObjects/TimeSpells/TimeSpellD", order = 1)]
public class RewindSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast Rewind");
        PlayerStateScript caster = Player.GetComponent<PlayerStateScript>();
        caster.applyAura(Player, aura_A, 5);
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        if (hitParticle != null)
        {
            //var particleBurst = Instantiate(hitParticle, Target);
            //particleBurst.Emit(10);
            FindObjectOfType<SpellRpcs>().SpawnParticleClientRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, slot, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, true);
        }
    }
}
