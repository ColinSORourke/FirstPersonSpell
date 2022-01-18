using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetUI : GenericUI
{
    // Start is called before the first frame update
    override public void Start()
    {
        UITrans = this.gameObject.transform.Find("InfoCanvas");
        UI = UITrans.GetComponent<Canvas>();
        HealthBar = UITrans.Find("Health").GetComponent<Image>();
        BonusBar = UITrans.Find("BonusHealth").GetComponent<Image>();
        ManaBar = UITrans.Find("Mana").GetComponent<Image>();
        UltBar = UITrans.Find("UltBar").GetComponent<Image>();
        Shield = UITrans.Find("Shield").GetComponent<Image>();
        TargetMark = UITrans.Find("TargetMarker").GetComponent<Image>();

        cameraToLookAt = Camera.main;
    }

    override public void Update(){
        var barTransf = UI.GetComponent<Transform>();
        barTransf.LookAt(cameraToLookAt.transform);
        barTransf.localRotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);
    }

    override public void updateCast(float percentage){
        // DO NOTHING
    }

    override public void shiftSpells(int slot, Sprite icon){
        // DO NOTHING
    }

    override public Image addIcon(Sprite icon, int slot){
        // DO NOTHING
        return null;
    }
}
