using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceSpellD", menuName = "ScriptableObjects/IceSpells/IceSpellD", order = 1)]
public class FrostApplySpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast FrostApply Spell");
        PlayerStateScript target = Target.GetComponent<PlayerStateScript>();
        float duration = 5.0f;
        target.applyAura(Player, aura_A, duration);
        if(slot == 0)
        {
            target.applyAura(Player, aura_A, duration);
        }

    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {

    }
}
