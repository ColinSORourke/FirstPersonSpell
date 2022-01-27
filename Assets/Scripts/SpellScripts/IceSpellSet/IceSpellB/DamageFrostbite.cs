using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceSpellB", menuName = "ScriptableObjects/IceSpells/IceSpellB", order = 1)]
public class DamageFrostbite : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast FrostB Spell");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        PlayerStateScript target = Target.GetComponent<PlayerStateScript>();
        target.takeDamage(damage);
        if (hitParticle != null)
        {
            var particleBurst = Instantiate(hitParticle, Target);
            particleBurst.Emit(10);
        }
        float duration = 5.0f;
        target.applyAura(Player, aura_A, duration);
        if (hitParticle != null)
        {
            Instantiate(hitParticle, Target);
        }
    }
}