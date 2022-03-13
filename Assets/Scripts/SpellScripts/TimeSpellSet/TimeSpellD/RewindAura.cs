using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewindAura", menuName = "ScriptableObjects/TimeSpells/RewindAura", order = 1)]
public class RewindAura : baseAuraScript
{
    
    public float saveMana;
    public float saveHealth;

    public Vector3 savePos;

    override public void onApply(Transform Player, Transform Target)
    {
        Debug.Log("Rewinding applied");

        savePos = Player.transform.position;
        Debug.Log(savePos.x);
        PlayerStateScript caster = Player.GetComponent<PlayerStateScript>();
        saveMana = caster.currMana;
        saveHealth = caster.currentHealth;

    }

    override public void onTick(Transform Player, Transform Target, int stack, int tickNum)
    {

    }

    override public void onExpire(Transform Player, Transform Target, int stack, int tickNum)
    {
        Debug.Log("It's rewind time");
        
        Debug.Log(savePos.x);
        Player.transform.position = savePos;
        PlayerStateScript caster = Player.GetComponent<PlayerStateScript>();
        caster.changeManaServerRpc(saveMana - caster.currMana);
        caster.changeHealthServerRpc(saveHealth - caster.currentHealth);
    }

    override public void onStack(Transform Player, Transform Target, int stack)
    {

    }
}
