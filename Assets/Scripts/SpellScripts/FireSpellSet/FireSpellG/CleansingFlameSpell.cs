using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSpellG", menuName = "ScriptableObjects/FireSpells/FireSpellG", order = 1)]
public class CleansingFlameSpell : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot){
        Debug.Log("Cast Cleansing Flame Spell");
        PlayerStateScript self = Player.GetComponent<PlayerStateScript>();
        int i = 0;
        for (i = 0; i <self.auras.Count; i++)
        {
            self.removeAura(i);
            self.changeUltServerRpc(1.0f);
        }
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        
        
    }
}