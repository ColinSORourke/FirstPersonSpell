using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodSpellB", menuName = "ScriptableObjects/BloodSpells/BloodSpellB", order = 1)]
public class SelfDamageBloodSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast Blood Spell B");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        PlayerStateScript tHP = Target.GetComponent<PlayerStateScript>();
        tHP.takeDamage(damage);
        PlayerStateScript pHP = Player.GetComponent<PlayerStateScript>();
        pHP.takeDamage(2.0f);
        float duration = 10.0f;
        if (tHP.hasAura(aura_A.id) == 1)
        {
                tHP.applyAura(Player, aura_A, duration); 
        }
        if (hitParticle != null)
        {
            var particleBurst = Instantiate(hitParticle, Target);
            particleBurst.Emit(10);
        }
    }
}