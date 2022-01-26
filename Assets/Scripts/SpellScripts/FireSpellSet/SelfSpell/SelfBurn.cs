using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelfBurn", menuName = "ScriptableObjects/FireSpells/SelfBurn", order = 1)]
public class SelfBurn : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot){
        // None
        Debug.Log("Cast Self burn spell");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        PlayerStateScript player = Player.GetComponent<PlayerStateScript>();
        player.applyAura(Player, aura_A, 10.0f);
    }
}