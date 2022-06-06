using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodSpellE", menuName = "ScriptableObjects/BloodSpells/BloodSpellE", order = 1)]
public class BleedBolt : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast Blood Spell E");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot, int index)
    {
        float duration = 10.0f;
        PlayerStateScript pHP = Player.GetComponent<PlayerStateScript>();
        PlayerStateScript tHP = Target.GetComponent<PlayerStateScript>();
        if (tHP.hasAura(aura_A.id) == 1)
        {
            pHP.changeHealthServerRpc(5.0f);
        }
        tHP.applyAura(Player, aura_A, duration);
        
        FindObjectOfType<SpellRpcs>().spawnHitParticleServerRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, index, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, true);
        this.playAudio(Target, "onHit");
    }
}