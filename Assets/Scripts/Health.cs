using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 50.0f;
    public float currentHealth = 50.0f;
    public float currentBonus = 20.0f;

    public List<liveAura> auras = new List<liveAura>();
    List<Image> auraIcons = new List<Image>();

    Image HealthBar;
    Image BonusBar;
    Image TargetMark;
    Camera cameraToLookAt;
    Canvas UI;

    
    // Start is called before the first frame update
    void Start()
    {
        UI = this.gameObject.transform.Find("InfoCanvas").GetComponent<Canvas>();
        HealthBar = this.gameObject.transform.Find("InfoCanvas").Find("Health").GetComponent<Image>();
        BonusBar = this.gameObject.transform.Find("InfoCanvas").Find("BonusHealth").GetComponent<Image>();
        TargetMark = this.gameObject.transform.Find("InfoCanvas").Find("TargetMarker").GetComponent<Image>();

        HealthBar.fillAmount = currentHealth/maxHealth;
        BonusBar.fillAmount = currentBonus/maxHealth;

        cameraToLookAt = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var barTransf = UI.GetComponent<Transform>();
        barTransf.LookAt(cameraToLookAt.transform);
        barTransf.localRotation = Quaternion.LookRotation(cameraToLookAt.transform.forward);

        int i = 0; 
        while (i < auras.Count){
            liveAura a = auras[i];
            int tickInfo = a.update(Time.deltaTime);
            if (tickInfo == -1){
                this.removeAura(i);
            } else {
                i += 1;
            }
        }
    }

    public void removeAura(int i){
        auras[i].onExpire();
        auras.RemoveAt(i);
        Destroy(auraIcons[i].gameObject);
        auraIcons.RemoveAt(i);
        int j = i; 
        while (j < auraIcons.Count){
            var auraTrans = auraIcons[j].GetComponent<Transform>();
            auraIcons[j].GetComponent<Transform>().localPosition = auraTrans.localPosition + new Vector3(-0.5f, 0, 0);
            j += 1;
        }
    }

    public void takeDamage(float dam){
        if (dam > currentBonus){
            dam -= currentBonus;
            currentBonus = 0;
            currentHealth -= dam;
        } else if (currentBonus > 0){
            currentBonus -= dam;
        } else {
            currentHealth -= dam;
        }
        HealthBar.fillAmount = currentHealth/maxHealth;
        BonusBar.fillAmount = currentBonus/maxHealth;

        if (currentHealth <= 0){
            GameObject player = GameObject.Find("Player");
            player.GetComponent<Targeting>().removeTarget(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void Target(){
        TargetMark.enabled = true;
    }

    public void unTarget(){
        TargetMark.enabled = false;
    }

    public void applyAura(Transform src, baseAuraScript aura, float duration){
        int matchInd = hasAura(aura.id);
        if (matchInd != -1){
            auras[matchInd].duration = duration;
            auras[matchInd].onStack();
        } else {
            liveAura toApply = new liveAura();
            toApply.aura = aura;
            toApply.on = this.transform;
            toApply.src = src;
            toApply.duration = duration;
            toApply.stacks = 1;
            toApply.tickNum = 0;
            toApply.onApply();
            auras.Add(toApply);
            if (aura.icon != null){
                auraIcons.Add(addIcon(toApply.aura.icon, toApply.aura.id, auraIcons.Count));
            }
        }
    }

    public Image addIcon(Sprite icon, int id, int count){
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
        return NewImage;
    }

    public int hasAura(int id){
        int i = 0;
        while (i < auras.Count){
            if (auras[i].aura.id == id){
                return i;
            }
            i += 1;
        }
        return -1;
    }
}
