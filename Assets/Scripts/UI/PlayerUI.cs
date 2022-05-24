using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : GenericUI
{

    public GameObject cardPrefab;

    public GameObject healthCount;// = new GameObject("HeathNum");

    public GameObject manaCount;

    public GameObject ultCount;

    public GameObject shieldCount;

    // Start is called before the first frame update
    public void Awake()
    {
        UITrans = this.gameObject.transform.Find("PlayerUI");
        UI = UITrans.GetComponent<Canvas>();
        HealthBar = UITrans.Find("Health").GetComponent<Image>();
        BonusBar = UITrans.Find("BonusHealth").GetComponent<Image>();
        ManaBar = UITrans.Find("Mana").GetComponent<Image>();
        CastBar = UITrans.Find("CastBar").GetComponent<Image>();
        UltBar = UITrans.Find("Ultimate").GetComponent<Image>();
        Shield = UITrans.Find("Shield").GetComponent<Image>();
        ShieldFill = Shield.transform.Find("Shield_Fill").GetComponent<Image>();
        healthCount = UITrans.Find("HealthText").gameObject;
        manaCount = UITrans.Find("ManaText").gameObject;
        ultCount = UITrans.Find("UltText").gameObject;
        shieldCount = UITrans.Find("ShieldText").gameObject;
        //Shield.enabled = false;
    }

    public override void updateMana(float currMana, float percentage){
        ManaBar.fillAmount = percentage * 0.86f;
        manaCount.GetComponent<Text>().text = currMana.ToString("n1");
    }

    public override void updateHealth(float currHealth, float currPerc, float bonusPerc){
        HealthBar.fillAmount = currPerc;
        BonusBar.fillAmount = bonusPerc;
        healthCount.GetComponent<Text>().text = ((currPerc + bonusPerc) * 50.0).ToString("n1");
    }

    public override void updateUlt(float currUlt, float percentage){
        UltBar.fillAmount = percentage;
        ultCount.GetComponent<Text>().text = currUlt.ToString("n1");
    }

    override public void displayShield(){
        Shield.enabled = true;
    }
    public override void updateShield(float currShield, float percentage)
    {
        ShieldFill.fillAmount = percentage;
        shieldCount.GetComponent<Text>().text = currShield.ToString();
    }

    public override void removeShield() {
        Shield.gameObject.SetActive(false);
        shieldCount.SetActive(false);
    }

    public override void addIcon(baseSpellScript spell, int slot){

        GameObject cardObj = Instantiate(cardPrefab, UITrans, false);
        var cardtrans = cardObj.GetComponent<RectTransform>();
        cardtrans.anchoredPosition = new Vector2(130 + (-140 * slot),60);
        if (slot == 3){
            cardtrans.anchoredPosition = new Vector2(130 + (-140 * slot),30);
        }
        cardObj.GetComponent<CardUI>().MatchSpell(spell);
        cardtrans.SetParent(UI.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        cardObj.SetActive(true);
        spellIcons[slot] = cardObj;
    }

    override public void target(){
        // THIS DOES NOTHING
    }

    override public void updateRange(float range){
        // THIS DOES NOTHING
    }

    override public void unTarget(){
        // THIS DOES NOTHING
    }
}
