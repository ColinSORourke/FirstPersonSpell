using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSpellC", menuName = "ScriptableObjects/FireSpells/FireSpellC", order = 1)]
public class damageBurnSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot){
        // None
        Debug.Log("Cast Damage Burn Spell");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        Health tHP = (Health) Target.GetComponent<Health>();
        tHP.takeDamage(damage);
        tHP.applyAura(Player, aura_A, 5.0f);
        
        if (hitParticle != null){
            Instantiate(hitParticle, Target);
        }
    }
}