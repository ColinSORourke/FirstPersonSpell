using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    public Text ManaCost;
    public Text CastTime;
    public Text Name;
    public Text DescText;
    public Image Icon;

    public void MatchSpell(baseSpellScript spell){
        ManaCost.text = spell.manaCost + "";
        CastTime.text = spell.castTime + "";
        //cardtrans.Find("RangeText").GetComponent<Text>().text = spell.range + "";
        Icon.sprite = spell.icon;
        Name.text = spell.spellName + "";
        DescText.text = spell.description + "";
    }
}
