using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSpellD", menuName = "ScriptableObjects/FireSpells/FireSpellD", order = 1)]
public class BurnManaSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot){
        // None
        Debug.Log("Cast Mana Fire Spell");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        Health tHP = (Health) Target.GetComponent<Health>();
        Mana pMana = Player.GetComponent<Mana>();
        tHP.takeDamage(this.damage);
        if (tHP.hasAura(aura_A.id) != -1){
            pMana.changeMana(10.0f);
            if (hitParticle != null){
                var particleBurst = Instantiate(hitParticle, Target);
                particleBurst.Emit(20);
            }
        }
        Debug.Log("Hit Mana Fire Spell");
    }
}
