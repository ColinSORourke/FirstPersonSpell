using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSpellE", menuName = "ScriptableObjects/FireSpells/FireSpellE", order = 1)]
public class BurnPayoffSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot){
        // None
        Debug.Log("Cast Burn Consume Spell");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        Health tHP = (Health) Target.GetComponent<Health>();
        float extraDamage = 0.0f;
        int burnInd = tHP.hasAura(aura_A.id);
        if (burnInd != -1){
            extraDamage = tHP.auras[burnInd].duration * 2;
            tHP.removeAura(burnInd);
        }
        tHP.takeDamage(this.damage + extraDamage);
        Debug.Log("Hit Burn Consume Spell");
    }
}