using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericUI : MonoBehaviour
{
    public List<Image> auraIcons = new List<Image>();
    public List<Text> auraStackText = new List<Text>();
    public List<Text> auraTimerText = new List<Text>();
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

    public virtual void updateHealth(float currHealth, float currPerc, float bonusPerc){
        HealthBar.fillAmount = currPerc;
        BonusBar.fillAmount = bonusPerc;
    }

    public virtual void updateMana(float currMana, float percentage){
        ManaBar.fillAmount = percentage;
    }

    public virtual void updateUlt(float currUlt, float percentage){
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

    public virtual void stackAura(liveAura stackedAura, int stackNum){
        GameObject aura = null;
        int location = -1;
        //Find the position in the list of the aura to be stacked
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
            txtTrans.localPosition = new Vector3( (0.5f * location),0.3f,0);
            txtTrans.localScale = new Vector3(0.02f, 0.02f, 1);
            txtTrans.sizeDelta = new Vector2(15, 15);
            textObject.SetActive(true);
            auraStackText.Add(stack);
        }
        else{
            auraStackText[location].gameObject.GetComponent<Text>().text = "x" + stackNum.ToString();
        }
        
        
    }

    public virtual void updateAura(int auraPos, liveAura updateAura){
        if (auraPos >= auraTimerText.Count){
            GameObject textObject = new GameObject("Aura" + updateAura.aura.id + " timer");
            Text stack = textObject.AddComponent<Text>();
            stack.text = updateAura.duration.ToString() + "s";
            stack.fontSize = 10;
            stack.font = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
            textObject.GetComponent<Text>().color = Color.black;

            var txtTrans = textObject.GetComponent<RectTransform>();
            txtTrans.SetParent(UI.transform); //Assign the newly created Text GameObject as a Child of the Parent Panel.
            txtTrans.localRotation = Quaternion.Euler(new Vector3(0,0,0));
            txtTrans.localPosition = new Vector3(-1f + (0.5f * auraPos),-0.1f,0);
            txtTrans.localScale = new Vector3(0.02f, 0.02f, 1);
            txtTrans.sizeDelta = new Vector2(30, 15);
            textObject.SetActive(true);
            auraTimerText.Add(stack);
        }
        else{
            auraTimerText[auraPos].gameObject.GetComponent<Text>().text = updateAura.duration.ToString() + "s";
        }
    }

    public virtual void removeAura(int i){
        //Only delete from stack List if Aura is stacked
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
            auraStackText[j].GetComponent<Transform>().localPosition = auraTrans.localPosition + new Vector3(-0.5f, 0.3f, 0);
            auraTimerText[j].GetComponent<Transform>().localPosition = auraTrans.localPosition + new Vector3(-3, -0.1f, 0);
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
