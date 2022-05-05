using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuraUI : MonoBehaviour
{
    public int id;
    public TMP_Text timer;
    public GameObject stacks;

    public void matchAuraApplication(Sprite icon, int i){
        this.GetComponent<Image>().sprite = icon;
        id = i;
    }

    public void updateTime(float time){
        timer.text = time.ToString() + "s";
    }

    public void updateStacks(int stacknum){
        if (stacknum > 1){
            stacks.SetActive(true);
            stacks.transform.GetChild(0).GetComponent<TMP_Text>().text = "x" + stacknum;
        }
    }
}
