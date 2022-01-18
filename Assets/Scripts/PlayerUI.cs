using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : GenericUI
{
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
        imgtrans.localPosition = new Vector3(-1 + (0.5f * count),0,0);
        imgtrans.sizeDelta = new Vector2(0.4f, 0.4f);
        imgObject.SetActive(true);
        auraIcons.Add(NewImage);
    }

    // Will have to change the positions
    override public void removeAura(int i){
        Destroy(auraIcons[i].gameObject);
        auraIcons.RemoveAt(i);
        int j = i; 
        while (j < auraIcons.Count){
            var auraTrans = auraIcons[j].GetComponent<Transform>();
            auraIcons[j].GetComponent<Transform>().localPosition = auraTrans.localPosition + new Vector3(-0.5f, 0, 0);
            j += 1;
        }
    }

    override public void target(){
        // THIS DOES NOTHING
    }

    override public void updateRange(){
        // THIS DOES NOTHING
    }

    override public void unTarget(){
        // THIS DOES NOTHING
    }
}
