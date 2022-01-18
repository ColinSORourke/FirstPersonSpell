using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSpellA", menuName = "ScriptableObjects/FireSpells/FireSpellA", order = 1)]
public class BasicFireSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot){
        // None
        Debug.Log("Cast Basic Fire Spell");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        Health tHP = (Health) Target.GetComponent<Health>();
        tHP.takeDamage(this.damage);
        Debug.Log("Hit Basic Fire Spell");
        if (hitParticle != null){
            var particleBurst = Instantiate(hitParticle, Target);
            particleBurst.Emit(10);
        }
    }
}
