using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericUI : MonoBehaviour
{
    public List<Image> auraIcons = new List<Image>();
    public Image HealthBar;
    public Image BonusBar;
    public Image TargetMark;
    public Camera cameraToLookAt;
    public Canvas UI;
    public Transform UITrans;
    public GameObject[] spellIcons = new GameObject[4];
    public Image CastBar;
    public Image ManaBar;
    public Image UltBar;
    public Image Shield;
    public Text ShieldCount;

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual void updateHealth(float currPerc, float bonusPerc){
        HealthBar.fillAmount = currPerc;
        BonusBar.fillAmount = bonusPerc;
    }

    public virtual void updateMana(float percentage){
        ManaBar.fillAmount = percentage;
    }

    public virtual void updateUlt(float percentage){
        UltBar.fillAmount = percentage;
    }

    public virtual void updateCast(float percentage){
        CastBar.fillAmount = percentage;
    }

    public virtual void shiftSpells(int slot, baseSpellScript spell){
        Destroy(spellIcons[slot].gameObject);
        int j = slot + 1; 
        while (j < spellIcons.Length){
            var spellTrans = spellIcons[j].GetComponent<RectTransform>();
            if (j == 3){spellIcons[j].GetComponent<RectTransform>().anchoredPosition = spellTrans.anchoredPosition + new Vector2(0, 30);

            }
            spellIcons[j].GetComponent<RectTransform>().anchoredPosition = spellTrans.anchoredPosition + new Vector2(100, 0);
            spellIcons[j].name = "Spell" + (j-1);
            spellIcons[j-1] = spellIcons[j];

            j += 1;
        }
        this.addIcon(spell, 3);
    }

    public virtual void addIcon(baseSpellScript spell, int slot){
        /* 
        To Implement in PlayerUI
        */
    }

    public virtual void displayShield(){
        Shield.enabled = true;
    }

    public virtual void removeShield(){
        Shield.enabled = false;
    }

    public virtual void addAura(Sprite icon, int id, int count){
        GameObject imgObject = new GameObject("Aura" + id); 
        //Create the GameObject
        Image NewImage = imgObject.AddComponent<Image>(); //Add the Image Component script
        NewImage.sprite = icon; //Set the Sprite of the Image Component on the new GameObject

        var imgtrans = imgObject.GetComponent<RectTransform>();
        imgtrans.SetParent(UI.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        imgtrans.localRotation = Quaternion.Euler(new Vector3(0,0,0));
        imgtrans.localPosition = new Vector3(-1 + (0.5f * count),0,0);
        imgtrans.sizeDelta = new Vector2(0.4f, 0.4f);
        imgObject.SetActive(true);
        auraIcons.Add(NewImage);
    }

    public virtual void removeAura(int i){
        Destroy(auraIcons[i].gameObject);
        auraIcons.RemoveAt(i);
        int j = i; 
        while (j < auraIcons.Count){
            var auraTrans = auraIcons[j].GetComponent<Transform>();
            auraIcons[j].GetComponent<Transform>().localPosition = auraTrans.localPosition + new Vector3(-0.5f, 0, 0);
            j += 1;
        }
    }

    public virtual void target(){
        TargetMark.enabled = true;
    }

    public virtual void updateRange(float range){
        if (range < 12.0){
            TargetMark.GetComponent<Image>().color = new Color32(251,249,13,255);
        }
        else if (range < 24.0){
            TargetMark.GetComponent<Image>().color = new Color32(250,143,13,255);
        }
        else{
            TargetMark.GetComponent<Image>().color = new Color32(217,18,10,255);
        }
        
    }

    public virtual void unTarget(){
        TargetMark.enabled = false;
    }
}
