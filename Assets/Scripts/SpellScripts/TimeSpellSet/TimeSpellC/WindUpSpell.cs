using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "Windup", menuName = "ScriptableObjects/TimeSpells/TimeSpellC", order = 1)]

public class WindUpSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast Windup");
        
        PlayerStateScript caster = Player.GetComponent<PlayerStateScript>();
        caster.changeManaServerRpc(10.0f);
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