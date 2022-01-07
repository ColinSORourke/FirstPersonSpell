using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSpellB", menuName = "ScriptableObjects/FireSpells/FireSpellB", order = 1)]
public class basicBurnSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot){
        // None
        Debug.Log("Cast Burn Spell");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        Health tHP = (Health) Target.GetComponent<Health>();
        float duration = 5.0f;
        if (tHP.hasAura(aura_A.id) != -1){
            duration = 15.0f;
        }
        tHP.applyAura(Player, aura_A, duration);
        if (hitParticle != null){
            Instantiate(hitParticle, Target);
        }
    }
}