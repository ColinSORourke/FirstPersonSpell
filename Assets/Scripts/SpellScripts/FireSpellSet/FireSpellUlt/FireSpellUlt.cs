using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSpellUlt", menuName = "ScriptableObjects/FireSpells/FireSpellUlt", order = 1)]
public class FireSpellUlt : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast Fire Spell Ult");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot, int index)
    {
        PlayerStateScript target = Target.GetComponent<PlayerStateScript>();
        target.takeDamage(damage);
        if (hitParticle != null)
        {
            //var particleBurst = Instantiate(hitParticle, Target);
            //particleBurst.Emit(10);
            FindObjectOfType<SpellRpcs>().SpawnParticleClientRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, index, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, true);
        }
        float duration = 10.0f;
        target.applyAura(Player, aura_A, duration);
    }
}
