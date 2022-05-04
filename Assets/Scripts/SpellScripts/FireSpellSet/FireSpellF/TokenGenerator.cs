using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSpellF", menuName = "ScriptableObjects/FireSpells/FireSpellF", order = 1)]
public class TokenGenerator : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot){
        // None
        Debug.Log("Cast Token Generator Spell");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot, int index)
    {
        PlayerStateScript target = Target.GetComponent<PlayerStateScript>();
        PlayerStateScript targeter = Player.GetComponent<PlayerStateScript>();
        //Targeting targeter = Player.GetComponent<Targeting>();
        target.takeDamage(this.damage);
        Debug.Log("Hit Basic Fire Spell");
        Debug.Log(targeter);
        if (slot == 0){
            targeter.spellQueue.Add(token);
            if (hitParticle != null){
                //var particleBurst = Instantiate(hitParticle, Player);
                //particleBurst.Emit(10);
                FindObjectOfType<SpellRpcs>().SpawnParticleClientRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, index, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId, true);
                Debug.Log("Emitted");
            }
        }
        
    }
}
