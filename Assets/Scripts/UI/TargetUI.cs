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
        RangeIndicator = UITrans.Find("RangeIndicator").GetComponent<Image>();
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

    override public void addAura(Sprite icon, int id, int count){
        GameObject auraObj = Instantiate(auraPrefab, UITrans, false);
        var auraTrans = auraObj.GetComponent<RectTransform>();
        auraTrans.anchoredPosition = auraTrans.anchoredPosition - new Vector2(0.45f * auraObjs.Count, 0);
        auraObj.GetComponent<AuraUI>().matchAuraApplication(icon, id);
        auraObj.GetComponent<AuraUI>().updateStacks(count);

        auraTrans.SetParent(UI.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        auraObjs.Add(auraObj);
    }

    override public void removeAura(int i){
        Destroy(auraObjs[i]);
        auraObjs.RemoveAt(i);

        int j = i; 
        while (j < auraObjs.Count){
            var auraTrans = auraObjs[j].GetComponent<RectTransform>();
            auraObjs[j].GetComponent<RectTransform>().anchoredPosition = auraTrans.anchoredPosition + new Vector2(0.45f, 0);
            j += 1;
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
