using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "ScriptableObjects/baseSpellScript", order = 1)]
public class baseSpellScript : ScriptableObject
{
    public int id;
    public float manaCost;
    public float castTime;
    public bool isProjectile;
    public float projSpeed;
    public GameObject projObj;
    public float damage;
    public Sprite icon;
    public bool reqTarget;

    public bool exhaust;
    public baseAuraScript aura_A;
    public baseAuraScript aura_B;
    public baseSpellScript token;

    public ParticleSystem castParticle;
    public ParticleSystem hitParticle;    

    public virtual void onCastGeneral(Transform Player, Transform Target, int slot){
        Mana pMana = (Mana) Player.GetComponent<Mana>();
        pMana.changeMana(this.manaCost * -1);
        if (castParticle != null){
            Instantiate(castParticle, Player);
        }

        this.onCastSpecific(Player, Target, slot);

        if (isProjectile){
            // Spawn Projectile that calls onHit upon collision
            Vector3 dir = Target.transform.position - Player.transform.position;
            dir.Normalize();
            var projectile = Instantiate(projObj, Player.transform.position + (dir * projSpeed), Quaternion.identity);
            var flyScript = projectile.AddComponent<ProjectileBehavior>();
            flyScript.spell = this;
            flyScript.Target = Target;
            flyScript.speed = projSpeed;
            flyScript.Source = Player;
            flyScript.slot = slot;
        } else {
            this.onHit(Player, Target, slot);
        }
    }

    public virtual void onCastSpecific(Transform Player, Transform Target, int slot){
        Debug.Log("Cast a basic spell");
    }

    public virtual void onHit(Transform Player, Transform Target, int slot){
        Health tHP = (Health) Target.GetComponent<Health>();
        tHP.takeDamage(this.damage);
        Debug.Log("Hit a basic spell");
        if (hitParticle != null){
            Instantiate(hitParticle, Target);
        }
       
    }
}
