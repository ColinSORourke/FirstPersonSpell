using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpellRpcs : NetworkBehaviour
{
    [ServerRpc(RequireOwnership = false)]
    public void SpawnProjectileServerRpc(ulong clientId, int slot, float posx, float posy, float posz, ulong targetId) {
        GameObject player = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject;
        baseSpellScript spell = player.GetComponent<PlayerStateScript>().spellQueue[slot];
        var projectile = Instantiate(spell.projObj, new Vector3(posx, posy, posz), Quaternion.identity);
        projectile.GetComponent<NetworkObject>().Spawn();
        var flyScript = projectile.AddComponent<ProjectileBehavior>();
        flyScript.spell = spell;
        flyScript.Target = NetworkManager.Singleton.ConnectedClients[targetId].PlayerObject.transform;
        flyScript.speed = spell.projSpeed;
        flyScript.Source = player.transform;
        flyScript.slot = slot;
        flyScript.lifespan = spell.projLifespan;
    }
}
