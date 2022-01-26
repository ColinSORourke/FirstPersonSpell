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
        PlayerStateScript target = Target.GetComponent<PlayerStateScript>();
        target.takeDamage(this.damage);
        if (target.hasAura(aura_A.id) != -1){
            target.changeMana(10.0f);
            //pMana.changeMana(10.0f);
            if (hitParticle != null){
                var particleBurst = Instantiate(hitParticle, Target);
                particleBurst.Emit(20);
            }
        }
        Debug.Log("Hit Mana Fire Spell");
    }
}
