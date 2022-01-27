using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceSpellF", menuName = "ScriptableObjects/IceSpells/IceSpellF", order = 1)]
public class IceStackSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast Snowball Spell");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        PlayerStateScript target = Target.GetComponent<PlayerStateScript>();
        target.takeDamage(damage);
        if (target.hasAura(aura_A.id) != -1)
        {
            int numStacks = target.auras[target.hasAura(aura_A.id)].stacks;
            target.takeDamage(3 * numStacks);
            target.removeAura(target.hasAura(aura_A.id));
        }
            if (hitParticle != null)
            {
                var particleBurst = Instantiate(hitParticle, Target);
                particleBurst.Emit(20);
            }
        
        Debug.Log("Hit Snowball Spell");
    }
}
