using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSpellC", menuName = "ScriptableObjects/FireSpells/FireSpellC", order = 1)]
public class flameBarrierSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot){
        // None
        Debug.Log("Cast Flame Barrier");
        
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot, int index)
    {
        PlayerStateScript player = Player.GetComponent<PlayerStateScript>();
        player.applyAura(Player, aura_B, 10.0f);
        //target.takeDamage(damage);
        this.playAudio(Player, "onHit");
    }
}