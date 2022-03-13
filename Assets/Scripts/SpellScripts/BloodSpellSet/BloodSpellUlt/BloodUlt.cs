using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BloodSpellUlt", menuName = "ScriptableObjects/BloodSpells/BloodSpellUlt", order = 1)]
public class BloodUlt : baseSpellScript
{
    // Start is called before the first frame update
    override public void onCastSpecific(Transform Player, Transform Target, int slot)
    {
        // None
        Debug.Log("Cast blood ult");
        PlayerStateScript target = Target.GetComponent<PlayerStateScript>();
        PlayerStateScript self = Player.GetComponent<PlayerStateScript>();
        target.takeDamage(damage);
        float extraDamage = 0.0f;
        if (target.hasAura(aura_A.id) != -1)
        {
            int numStacks = target.auras[target.hasAura(aura_A.id)].stacks;
            extraDamage = 2.0f * numStacks;
            target.takeDamage(extraDamage);
        }
        self.changeBonusServerRpc(damage + extraDamage);
        self.changeManaServerRpc(damage);
        if (self.currMana > 50.0f) //caps mana gained to max
        {
            self.changeManaServerRpc(-(self.currMana - 50.0f));
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
