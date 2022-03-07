using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceSpellC", menuName = "ScriptableObjects/IceSpells/IceSpellC", order = 1)]
public class DefenseIceSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast Frost Shield Spell");
        PlayerStateScript player = Player.GetComponent<PlayerStateScript>();
        player.changeBonusServerRpc(15.0f);
        //player.myUI.updateHealth(player.currentHealth / player.maxHealth, player.currentBonus / player.maxHealth);
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {

    }
}