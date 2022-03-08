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


    // Start is called before the first frame update
    public override void Start()
    {
        UITrans = this.gameObject.transform.Find("PlayerUI");
        UI = UITrans.GetComponent<Canvas>();
        HealthBar = UITrans.Find("Health").GetComponent<Image>();
        BonusBar = UITrans.Find("BonusHealth").GetComponent<Image>();
        ManaBar = UITrans.Find("Mana").GetComponent<Image>();
        CastBar = UITrans.Find("CastBar").GetComponent<Image>();
        UltBar = UITrans.Find("Ultimate").GetComponent<Image>();
        Shield = UITrans.Find("Shield").GetComponent<Image>();
        Shield.enabled = false;

        healthCount = new GameObject("HeathNum");
        Text health = healthCount.AddComponent<Text>();
        health.text = "50.0";
        health.fontSize = 14;
        health.fontStyle = FontStyle.Bold;
        health.font = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
        healthCount.GetComponent<Text>().color = Color.white;

        var healthTrans = healthCount.GetComponent<RectTransform>();
        healthTrans.SetParent(UI.transform); //Assign the newly created Text GameObject as a Child of the Parent Panel.
        healthTrans.localRotation = Quaternion.Euler(new Vector3(0,0,0));
        healthTrans.localPosition = UITrans.Find("Health").localPosition + new Vector3(4,29,0);
        //healthTrans.localScale = new Vector3(1,1,1);
        healthTrans.sizeDelta = new Vector2(40, 20);
        healthCount.SetActive(true);

        manaCount = new GameObject("ManaNum");
        Text mana = manaCount.AddComponent<Text>();
        mana.text = "50.0";
        mana.fontSize = 14;
        mana.fontStyle = FontStyle.Bold;
        mana.font = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
        manaCount.GetComponent<Text>().color = Color.white;

        var manaTrans = manaCount.GetComponent<RectTransform>();
        manaTrans.SetParent(UI.transform); //Assign the newly created Text GameObject as a Child of the Parent Panel.
        manaTrans.localRotation = Quaternion.Euler(new Vector3(0,0,0));
        manaTrans.localPosition = UITrans.Find("Mana").localPosition + new Vector3(4,28,0);
        //manaTrans.localScale = new Vector3(1,1,1);
        manaTrans.sizeDelta = new Vector2(45, 20);
        manaCount.SetActive(true);

        ultCount= new GameObject("UltNum");
        Text ult = ultCount.AddComponent<Text>();
        ult.text = "0.0";
        ult.fontSize = 14;
        ult.fontStyle = FontStyle.Bold;
        ult.font = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
        ultCount.GetComponent<Text>().color = Color.white;

        var ultTrans = ultCount.GetComponent<RectTransform>();
        ultTrans.SetParent(UI.transform); //Assign the newly created Text GameObject as a Child of the Parent Panel.
        ultTrans.localRotation = Quaternion.Euler(new Vector3(0,0,0));
        ultTrans.localPosition = UITrans.Find("Ultimate").localPosition + new Vector3(7,27,0);
        //ultTrans.localScale = new Vector3(1,1,1);
        ultTrans.sizeDelta = new Vector2(40, 20);
        ultCount.SetActive(true);


    }

    public override void updateMana(float currMana, float percentage){
        ManaBar.fillAmount = percentage * 0.86f;
        manaCount.GetComponent<Text>().text = currMana.ToString("n1");
    }

    public override void updateHealth(float currHealth, float currPerc, float bonusPerc){
        HealthBar.fillAmount = currPerc;
        BonusBar.fillAmount = bonusPerc;
        healthCount.GetComponent<Text>().text = currHealth.ToString("n1");
    }

    public override void updateUlt(float currUlt, float percentage){
        UltBar.fillAmount = percentage;
        ultCount.GetComponent<Text>().text = currUlt.ToString("n1");
    }

    override public void displayShield(){
        Shield.enabled = true;
    }

    // Will have to change the positions & sizes for the player UI
    override public void addAura(Sprite icon, int id, int count){
        GameObject imgObject = new GameObject("Aura" + id); 
        //Create the GameObject
        Image NewImage = imgObject.AddComponent<Image>(); //Add the Image Component script
        NewImage.sprite = icon; //Set the Sprite of the Image Component on the new GameObject

        var imgtrans = imgObject.GetComponent<RectTransform>();
        imgtrans.SetParent(UI.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        imgtrans.localRotation = Quaternion.Euler(new Vector3(0,0,0));
        imgtrans.localPosition = new Vector3(-1 + (120.0f * count),0,0);
        imgtrans.sizeDelta = new Vector2(100.0f, 100.0f);
        imgObject.SetActive(true);
        auraIcons.Add(NewImage);
    }

    override public void stackAura(liveAura stackedAura, int stackNum){
        GameObject aura = null;
        int location = -1;
        for (int i = 0; i < auraIcons.Count; i++){
            if (auraIcons[i].gameObject.name == ("Aura" + stackedAura.aura.id)){
                aura = auraIcons[i].gameObject;
                location = i;
            }
        }

        if (auraStackText.Count <= location){
            GameObject textObject = new GameObject("Aura" + stackedAura.aura.id + " stack");
            Text stack = textObject.AddComponent<Text>();
            stack.text = "x" + stackNum.ToString();
            stack.fontSize = 10;
            stack.font = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
            textObject.GetComponent<Text>().color = Color.black;

            var txtTrans = textObject.GetComponent<RectTransform>();
            txtTrans.SetParent(UI.transform); //Assign the newly created Text GameObject as a Child of the Parent Panel.
            txtTrans.localRotation = Quaternion.Euler(new Vector3(0,0,0));
            txtTrans.localPosition = new Vector3(-1.5f + (120f * location),10f,0);
            txtTrans.localScale = new Vector3(2f, 2f, 1);
            txtTrans.sizeDelta = new Vector2(15, 15);
            textObject.SetActive(true);
            auraStackText.Add(stack);
        }
        else{
            auraStackText[location].gameObject.GetComponent<Text>().text = "x" + stackNum.ToString();
        }
        
    }

    override public void updateAura(int auraPos, liveAura updateAura){
        if (auraPos >= auraTimerText.Count){
            GameObject textObject = new GameObject("Aura" + updateAura.aura.id + " timer");
            Text time = textObject.AddComponent<Text>();
            time.text = updateAura.duration.ToString() + "s";
            time.fontSize = 10;
            time.font = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
            textObject.GetComponent<Text>().color = Color.black;

            var txtTrans = textObject.GetComponent<RectTransform>();
            txtTrans.SetParent(UI.transform); //Assign the newly created Text GameObject as a Child of the Parent Panel.
            txtTrans.localRotation = Quaternion.Euler(new Vector3(0,0,0));
            txtTrans.localPosition = new Vector3(-3f + (120f * auraPos),-10f,0);
            txtTrans.localScale = new Vector3(2f, 2f, 1);
            txtTrans.sizeDelta = new Vector2(15, 15);
            textObject.SetActive(true);
            auraTimerText.Add(time);
        }
        else{
            auraTimerText[auraPos].gameObject.GetComponent<Text>().text = updateAura.duration.ToString() + "s";
        }
    }

    // Will have to change the positions
    override public void removeAura(int i){
        
        if (i < auraStackText.Count){
            var stackTxt = auraStackText[i].gameObject.name;
            if(stackTxt.Substring(0, stackTxt.Length - 6) == auraIcons[i].gameObject.name){
                Destroy(auraStackText[i].gameObject);
                auraStackText.RemoveAt(i);
            } 
        } 
        Destroy(auraIcons[i].gameObject);
        auraIcons.RemoveAt(i);
        Destroy(auraTimerText[i].gameObject);
        auraTimerText.RemoveAt(i);
        
        int j = i; 
        while (j < auraIcons.Count){
            var auraTrans = auraIcons[j].GetComponent<Transform>();
            auraIcons[j].GetComponent<Transform>().localPosition = auraTrans.localPosition + new Vector3(-0.5f, 0, 0);
            auraStackText[j].GetComponent<Transform>().localPosition = auraTrans.localPosition + new Vector3(-0.5f, 10f, 0);
            auraTimerText[j].GetComponent<Transform>().localPosition = auraTrans.localPosition + new Vector3(-3, -0.4f, 0);
            j += 1;
        }
    }

    public override void addIcon(baseSpellScript spell, int slot){

        GameObject cardObj = Instantiate(cardPrefab, cardPrefab.transform.position, Quaternion.identity);

        cardObj.transform.SetParent(UITrans);
        //Create the GameObject

        

        var cardtrans = cardObj.GetComponent<RectTransform>();
        cardtrans.anchoredPosition = new Vector2(130 + (-100 * slot),60);
        if (slot == 3){
            cardtrans.anchoredPosition = new Vector2(130 + (-100 * slot),30);
        }
        cardtrans.Find("ManaCost").GetComponent<Text>().text = spell.manaCost + "";
        cardtrans.Find("CastTime").GetComponent<Text>().text = spell.castTime + "\nsec";
        cardtrans.Find("RangeText").GetComponent<Text>().text = spell.range + "";
        cardtrans.Find("Image").GetComponent<Image>().sprite = spell.icon;
        cardtrans.Find("NameText").GetComponent<Text>().text = spell.spellName + "";
        cardtrans.Find("DescriptionText").GetComponent<Text>().text = spell.description + "";
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
