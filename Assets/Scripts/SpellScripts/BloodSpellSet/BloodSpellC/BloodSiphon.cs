using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodSpellC", menuName = "ScriptableObjects/BloodSpells/BloodSpellC", order = 1)]
public class BloodSiphon : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast blood siphon");
        PlayerStateScript target = Target.GetComponent<PlayerStateScript>();
        PlayerStateScript self = Player.GetComponent<PlayerStateScript>();
        target.takeDamage(damage);
        self.changeHealthServerRpc(5.0f);
        if (slot == 0)
        {
            self.changeHealthServerRpc(5.0f);
        }

    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot, int index)
    {
        if (hitParticle != null)
        {
            //var particleBurst = Instantiate(hitParticle, Target);
            //particleBurst.Emit(10);
            FindObjectOfType<SpellRpcs>().SpawnParticleClientRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, index, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, true);
        }
    }
}
