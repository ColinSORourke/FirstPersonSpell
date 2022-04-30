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
    public Image Range;

    public Sprite[] cardBacks = new Sprite[4];
    public Sprite[] rangeIcons = new Sprite[3];

    public void MatchSpell(baseSpellScript spell){
        this.transform.GetComponent<Image>().sprite = cardBacks[(spell.id/100) - 1 ];
        if ( (((int)spell.range)/15) - 1 >= 0){
            Range.sprite = rangeIcons[ (((int)spell.range)/15) - 1];
        }

        ManaCost.text = spell.manaCost + "";
        CastTime.text = spell.castTime + "";
        //cardtrans.Find("RangeText").GetComponent<Text>().text = spell.range + "";
        Icon.sprite = spell.icon;
        Name.text = spell.spellName + "";
        DescText.text = "\t" + spell.description;
    }
}
