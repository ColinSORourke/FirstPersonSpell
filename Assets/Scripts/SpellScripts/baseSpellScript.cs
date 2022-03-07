using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "ScriptableObjects/baseSpellScript", order = 1)]
public class baseSpellScript : ScriptableObject
{
    public int id;
    public float manaCost;
    public float castTime;

    public bool isProjectile;
    public float projSpeed;
    public float projLifespan;
    public GameObject projObj;

    public float damage;
    public float ultCost;
    public float range;
    public bool reqTarget;
    public bool exhaust;
    public baseAuraScript aura_A;
    public baseAuraScript aura_B;
    public baseSpellScript token;

    public Sprite icon;
    public ParticleSystem castParticle;
    public ParticleSystem hitParticle;    

    public virtual void onCastGeneral(Transform Player, Transform Target, int slot){
        if (castParticle != null){
            Instantiate(castParticle, Player);
        }
        this.onCastSpecific(Player, Target, slot);

        if (isProjectile){
            // Spawn Projectile that calls onHit upon collision
            Vector3 dir = Target.transform.position - Player.transform.position;
            dir.Normalize();
            Vector3 pos = Player.transform.position + (dir * projSpeed);
            FindObjectOfType<SpellRpcs>().SpawnProjectileServerRpc(NetworkManager.Singleton.LocalClientId, slot, pos.x, pos.y, pos.z, Target.GetComponent<NetworkObject>().OwnerClientId);
        } else {
            if (reqTarget){
                if( ! Target.GetComponent<PlayerStateScript>().isShielded() ){
                    this.onHit(Player, Target, slot);
                }
            } else {
                this.onHit(Player, Target, slot);
            }
            
        }
    }

    public virtual void onCastSpecific(Transform Player, Transform Target, int slot){
        Debug.Log("Cast a basic spell");
    }

    public virtual void onHit(Transform Player, Transform Target, int slot){
        Debug.Log("Hit a basic spell");
        if (hitParticle != null){
            FindObjectOfType<SpellRpcs>().SpawnParticleServerRpc(Player.gameObject.GetComponent<NetworkObject>().OwnerClientId, slot, Target.gameObject.GetComponent<NetworkObject>().OwnerClientId);
        }
       
    }
}
