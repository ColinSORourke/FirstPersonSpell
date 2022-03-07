using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpellRpcs : NetworkBehaviour
{
    public List<GameObject> projectiles;
    
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

    [ClientRpc]
    public void DestroyProjectileClientRpc(ulong sourceId, ulong targetId, int projectileIndex, int slot) {
        if (NetworkManager.Singleton.LocalClientId == targetId) {
            Debug.Log("Spell Hit");
            GameObject sourcePlayer = GameObject.Find("Player " + sourceId);
            GameObject targetPlayer = GameObject.Find("Player " + targetId);
            //GameObject.Destroy(projectiles[projectileIndex]);
            //projectiles[projectileIndex].GetComponent<MeshRenderer>().enabled = false;
            //projectiles[projectileIndex].GetComponent<Collider>().enabled = false;
            DestroyProjectileServerRpc(projectileIndex);
            if (!targetPlayer.GetComponent<PlayerStateScript>().isShielded()) {
                sourcePlayer.GetComponent<PlayerStateScript>().spellQueue[slot].onHit(sourcePlayer.transform, targetPlayer.transform, slot);
            }
        }

        if (!IsHost) {
            projectiles[projectileIndex] = null;
            if(NoProjectiles()) projectiles.Clear();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyProjectileServerRpc(int projectileIndex) {
        GameObject.Destroy(projectiles[projectileIndex]);
        projectiles[projectileIndex] = null;
        if (NoProjectiles()) projectiles.Clear();
    }

    public bool NoProjectiles() {
        foreach(var projectile in projectiles) {
            if(projectile != null) {
                return false;
            }
        }
        return true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnParticleServerRpc(ulong clientId, int slot, ulong targetId) {
        GameObject player = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject;
        baseSpellScript spell = player.GetComponent<PlayerStateScript>().spellQueue[slot];
        var particle = Instantiate(spell.hitParticle, NetworkManager.Singleton.ConnectedClients[targetId].PlayerObject.transform);
        particle.GetComponent<NetworkObject>().Spawn();
    }
}
