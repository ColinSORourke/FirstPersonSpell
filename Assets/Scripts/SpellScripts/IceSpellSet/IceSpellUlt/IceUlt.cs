using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "IceUlt", menuName = "ScriptableObjects/IceSpells/IceSpellUlt", order = 1)]
public class IceUlt : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast FrostB Spell");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot, int index)
    {
        PlayerStateScript target = Target.GetComponent<PlayerStateScript>();
        target.takeDamage(damage);
        if (hitParticle != null)
        {
            var particleBurst = Instantiate(hitParticle, Target);
            particleBurst.Emit(10);
        }
        float duration = 15.0f;
        target.applyAura(Player, aura_A, duration);
        FindObjectOfType<SpellRpcs>().spawnHitParticleServerRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, index, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, false);
        this.playAudio(Target, "onHit");
    }
}
