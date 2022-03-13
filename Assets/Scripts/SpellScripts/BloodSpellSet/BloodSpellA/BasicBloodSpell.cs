using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodSpellA", menuName = "ScriptableObjects/BloodSpells/BloodSpellA", order = 1)]
public class BasicBloodSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast Blood Spell A");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        PlayerStateScript tHP = Target.GetComponent<PlayerStateScript>();
        tHP.takeDamage(damage);
        if (hitParticle != null)
        {
            //var particleBurst = Instantiate(hitParticle, Target);
            //particleBurst.Emit(10);
            FindObjectOfType<SpellRpcs>().SpawnParticleClientRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, slot, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, true);
        }
    }
}