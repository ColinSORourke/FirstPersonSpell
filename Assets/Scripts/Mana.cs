using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour
{
    public float maxMana = 50.0f;
    public float currMana = 50.0f;
    Image ManaBar;

    // Start is called before the first frame update
    void Start()
    {
        ManaBar = this.gameObject.transform.Find("SpellUI").Find("Mana").GetComponent<Image>();
        InvokeRepeating("manaTick", 2.0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void manaTick(){
        if (currMana > 0){
            //currMana -= 0.5f;
            ManaBar.fillAmount = currMana/maxMana;
        }
    }

    public void changeMana(float value){
        currMana += value;
        currMana = Mathf.Clamp(currMana, 0, maxMana);
        Debug.Log(currMana);
        ManaBar.fillAmount = currMana/maxMana;
    }
}
