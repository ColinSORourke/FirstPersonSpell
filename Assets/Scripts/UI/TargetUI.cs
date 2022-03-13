using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetUI : GenericUI
{
    // Start is called before the first frame update
    public void Awake()
    {
        UITrans = this.gameObject.transform.Find("InfoCanvas");
        UI = UITrans.GetComponent<Canvas>();
        HealthBar = UITrans.Find("Health").GetComponent<Image>();
        BonusBar = UITrans.Find("BonusHealth").GetComponent<Image>();
        ManaBar = UITrans.Find("Mana").GetComponent<Image>();
        UltBar = UITrans.Find("UltBar").GetComponent<Image>();
        Shield = UITrans.Find("Shield").GetComponent<Image>();
        TargetMark = UITrans.Find("TargetMarker").GetComponent<Image>();
        Shield.enabled = false;
    }

    override public void Update(){
        var barTransf = UI.GetComponent<Transform>();
        if (cameraToLookAt != null) {
            barTransf.LookAt(cameraToLookAt.transform);
            barTransf.rotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
        } else {
            cameraToLookAt = Camera.main;
        }
    }

    override public void updateCast(float percentage){
        // DO NOTHING
    }

    override public void shiftSpells(int slot, baseSpellScript spell){
        // DO NOTHING
    }

    override public void addIcon(baseSpellScript spell, int slot){
        // DO NOTHING
    }
}
