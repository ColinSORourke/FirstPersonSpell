using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSpellE", menuName = "ScriptableObjects/FireSpells/FireSpellE", order = 1)]
public class BurnPayoffSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot){
        // None
        Debug.Log("Cast Burn Consume Spell");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        PlayerStateScript target = Target.GetComponent<PlayerStateScript>();
        float extraDamage = 0.0f;
        int burnInd = target.hasAura(aura_A.id);
        if (burnInd != -1){
            extraDamage = target.auras[burnInd].duration * 2;
            target.removeAura(burnInd);
        }
        target.takeDamage(this.damage + extraDamage);
        Debug.Log("Hit Burn Consume Spell");

        if (hitParticle != null)
        {
            //var particleBurst = Instantiate(hitParticle, Target);
            //particleBurst.Emit(10);
            FindObjectOfType<SpellRpcs>().SpawnParticleClientRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, slot, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, true);
        }
    }
}