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
    public Image Back;

    public Sprite[] cardBacks = new Sprite[5];
    public Sprite[] rangeIcons = new Sprite[3];
    public Color32[] rangeColors = new Color32[] { new Color32(84,255,95,255), new Color32(251,249,13,255), new Color32(250,143,13,255) };

    public void MatchSpell(baseSpellScript spell, bool ult = false){
        Back.sprite = cardBacks[(spell.id/100) - 1 ];
        if (spell.exhaust){
            Back.sprite = cardBacks[4];
        }
        if ( (((int)spell.range)/15) - 1 >= 0){
            Range.sprite = rangeIcons[ (((int)spell.range)/15) - 1];
            //Range.color = rangeColors[ (((int)spell.range)/15) - 1];
        }

        ManaCost.text = spell.manaCost + "";
        CastTime.text = spell.castTime + "";
        //cardtrans.Find("RangeText").GetComponent<Text>().text = spell.range + "";
        Icon.sprite = spell.icon;
        Name.text = spell.spellName + "";
        DescText.text = spell.description;
    }

    public IEnumerator HighlightCard(bool valid, float duration = 0.5f)
    {
        GameObject highlight = this.transform.Find("Highlight").Find(valid ? "Valid" : "Invalid").gameObject;
        if (highlight == null) Debug.Log("Highlight not found");
        highlight.SetActive(true);
        yield return new WaitForSeconds(duration);
        if (highlight != null){
            highlight.SetActive(false);
        }
    }
}
