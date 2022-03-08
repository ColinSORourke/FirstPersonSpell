using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSpellC", menuName = "ScriptableObjects/FireSpells/FireSpellC", order = 1)]
public class flameBarrierSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot){
        // None
        Debug.Log("Cast Flame Barrier Spell");
        PlayerStateScript p = Target.GetComponent<PlayerStateScript>();
        p.applyAura(Player, aura_B, 10.0f);
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
       
        
        
    }
}