using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodSpellF", menuName = "ScriptableObjects/BloodSpells/BloodSpellF", order = 1)]
public class SelfBleedSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast Blood Spell B");
        
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot, int index)
    {
        PlayerStateScript tHP = Target.GetComponent<PlayerStateScript>();
 
        PlayerStateScript pHP = Player.GetComponent<PlayerStateScript>();

        float duration = 10.0f;
        pHP.applyAura(Player, aura_A, duration);
        tHP.applyAura(Player, aura_A, duration);
        tHP.applyAura(Player, aura_A, duration);

        if (pHP.currentHealth <= 10.0f)
        {
            tHP.applyAura(Player, aura_A, duration);
        }
        FindObjectOfType<SpellRpcs>().spawnHitParticleServerRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, index, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, true);
        this.playAudio(Target, "onHit");
    }
}