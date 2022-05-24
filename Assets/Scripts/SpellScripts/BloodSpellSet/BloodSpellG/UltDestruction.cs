using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodSpellG", menuName = "ScriptableObjects/BloodSpells/BloodSpellG", order = 1)]
public class UltDestruction : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast Blood Spell G");

        

    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot, int index)
    {
        PlayerStateScript tHP = Target.GetComponent<PlayerStateScript>();
        PlayerStateScript pHP = Player.GetComponent<PlayerStateScript>();
        pHP.takeDamage(2.0f);
        if(tHP.currUlt > 2.0f)
        {
            tHP.changeUltServerRpc(-2.0f);
        }
        else if(tHP.currUlt == 1.0f)
        {
            tHP.changeUltServerRpc(-1.0f);
        }
        pHP.changeUltServerRpc(2.0f);

        FindObjectOfType<SpellRpcs>().spawnHitParticleServerRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, index, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, true);

    }
}