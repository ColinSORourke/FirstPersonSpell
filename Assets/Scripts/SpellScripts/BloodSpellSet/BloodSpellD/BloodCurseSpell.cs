using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodSpellD", menuName = "ScriptableObjects/BloodSpells/BloodSpellD", order = 1)]
public class BloodCurseSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast BloodCurse Spell");
        PlayerStateScript target = Target.GetComponent<PlayerStateScript>();
        float duration = 10.0f;
        target.applyAura(Player, aura_B, duration);
        if(slot == 2)
        {
            target.applyAura(Player, aura_B, duration);
        }

    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        if (hitParticle != null)
        {
            var particleBurst = Instantiate(hitParticle, Target);
            particleBurst.Emit(10);
        }
    }
}
