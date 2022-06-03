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
        
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot, int index)
    {
        PlayerStateScript caster = Player.GetComponent<PlayerStateScript>();
        caster.applyAura(Player, aura_A, 5);
        FindObjectOfType<SpellRpcs>().spawnHitParticleServerRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, slot, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, true);
        this.playAudio(Player, "onHit");
    }
}
