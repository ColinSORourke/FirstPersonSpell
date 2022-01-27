using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceSpellA", menuName = "ScriptableObjects/IceSpells/IceSpellA", order = 1)]
public class BasicIceSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast Ice Spell A");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        PlayerStateScript tHP = Target.GetComponent<PlayerStateScript>();
        tHP.takeDamage(damage);
        if (hitParticle != null)
        {
            var particleBurst = Instantiate(hitParticle, Target);
            particleBurst.Emit(10);
        }
    }
}