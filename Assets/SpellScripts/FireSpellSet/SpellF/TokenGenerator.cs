using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireSpellF", menuName = "ScriptableObjects/FireSpells/FireSpellF", order = 1)]
public class TokenGenerator : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot){
        // None
        Debug.Log("Cast Token Generator Spell");
    }

    // Update is called once per frame
    override public void onHit(Transform Player, Transform Target, int slot)
    {
        Health tHP = (Health) Target.GetComponent<Health>();
        Targeting targeter = Player.GetComponent<Targeting>();
        tHP.takeDamage(this.damage);
        Debug.Log("Hit Basic Fire Spell");
        Debug.Log(targeter);
        if (slot == 0){
            targeter.spellQueue.Add(token);
            if (hitParticle != null){
                var particleBurst = Instantiate(hitParticle, Player);
                particleBurst.Emit(10);
            }
        }
        
    }
}
