using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceSpellG", menuName = "ScriptableObjects/IceSpells/IceSpellG", order = 1)]
public class IceRamperSpell : baseSpellScript
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
            PlayerStateScript player = Player.GetComponent<PlayerStateScript>();
            player.currUlt += 1.0f;
        }
        if (hitParticle != null)
        {
            var particleBurst = Instantiate(hitParticle, Target);
            particleBurst.Emit(20);
        }

        Debug.Log("Hit Snowball Spell");
    }
}

