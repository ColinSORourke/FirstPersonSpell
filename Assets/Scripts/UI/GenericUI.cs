using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericUI : MonoBehaviour
{
    public GameObject auraPrefab;
    public List<GameObject> auraObjs = new List<GameObject>();

    public Image HealthBar;
    public Image BonusBar;
    public Camera cameraToLookAt;
    public Canvas UI;
    public Transform UITrans;
    public GameObject[] spellIcons = new GameObject[4];
    public Image CastBar;
    public Image ManaBar;
    public Image UltBar;
    public Image Shield;
    public Image ShieldFill;
    public GameObject TargetMark;
    public Image RangeIndicator;
    public Sprite RangeLow;
    public Sprite RangeMid;
    public Sprite RangeHigh;
    public Sprite RangeNo;
    public GameObject spinningUlt;

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

    public virtual void updateShield(float currShield, float percentage){
        ShieldFill.fillAmount = percentage;
    }

    public IEnumerator MoveOverTime (GameObject objectToMove, Vector2 end, float seconds){
        float elapsedTime = 0;
        Vector2 startingPos = objectToMove.GetComponent<RectTransform>().anchoredPosition;
        while (elapsedTime < seconds){
            objectToMove.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(startingPos, end, (elapsedTime / seconds)); 
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.GetComponent<RectTransform>().anchoredPosition = end;
    }
    public virtual void shiftSpells(int slot, baseSpellScript spell){
        Destroy(spellIcons[slot].gameObject);
        int j = slot + 1; 
        while (j < spellIcons.Length){
            StartCoroutine( MoveOverTime(spellIcons[j], new Vector2(130, 60)  + new Vector2(140, 0) * (1-j), 0.80f));
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

        GameObject auraObj = Instantiate(auraPrefab, UITrans, false);
        var auraTrans = auraObj.GetComponent<RectTransform>();
        auraTrans.anchoredPosition = auraTrans.anchoredPosition + new Vector2(0,80 * auraObjs.Count);
        auraObj.GetComponent<AuraUI>().matchAuraApplication(icon, id);
        auraObj.GetComponent<AuraUI>().updateStacks(count);

        auraTrans.SetParent(UI.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        auraObjs.Add(auraObj);
    }

    public virtual void stackAura(liveAura stackedAura, int stackNum){
        GameObject aura = null;
        int location = -1;
        //Find the position in the list of the aura to be stacked
        for (int i = 0; i < auraObjs.Count; i++){
            if (auraObjs[i].GetComponent<AuraUI>().id == stackedAura.aura.id){
                aura = auraObjs[i];
                location = i;
            }
        }
        aura.GetComponent<AuraUI>().updateStacks(stackNum);
    }

    public virtual void updateAura(liveAura updateAura){
        GameObject aura = null;
        int location = -1;
        //Find the position in the list of the aura to be stacked
        for (int i = 0; i < auraObjs.Count; i++){
            if (auraObjs[i].GetComponent<AuraUI>().id == updateAura.aura.id){
                aura = auraObjs[i];
                location = i;
            }
        }
        aura.GetComponent<AuraUI>().updateTime(updateAura.duration);
    }

    public virtual void removeAura(int i){
        Destroy(auraObjs[i]);
        auraObjs.RemoveAt(i);

        int j = i; 
        while (j < auraObjs.Count){
            var auraTrans = auraObjs[j].GetComponent<RectTransform>();
            auraObjs[j].GetComponent<RectTransform>().anchoredPosition = auraTrans.anchoredPosition - new Vector2(0, 80);
            j += 1;
        }
    }


    public virtual void target(){
        TargetMark.SetActive(true);
    }

    public virtual void updateRange(float range){
        if (range < 15.0){
            RangeIndicator.color = new Color32(84,255,95,255);
            RangeIndicator.sprite = RangeLow;
        }
        else if (range < 30.0){
            RangeIndicator.color = new Color32(251,249,13,255);
            RangeIndicator.sprite = RangeMid;
        }
        else if (range < 45.0) {
            RangeIndicator.color = new Color32(250,143,13,255);
            RangeIndicator.sprite = RangeHigh;
        } else {
            RangeIndicator.color = new Color32(217,18,10,255);
            RangeIndicator.sprite = RangeNo;
        }
        
    }

    public virtual void unTarget(){
        TargetMark.SetActive(false);
    }

    public virtual void getUlt(){
        spinningUlt.SetActive(true);
        Invoke("disableUlt", 3.5f);
    }

    public virtual void disableUlt(){
        spinningUlt.SetActive(false);
    }
}
