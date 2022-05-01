using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDeckDisplay : MonoBehaviour
{
    public SpellDeck defaultDeck;
    public SpellDeck[] decks;

    public void Start(){
        this.displayDeck(defaultDeck);
    }

    public void displayDeck(SpellDeck d){
        this.transform.GetChild(0).GetComponent<CardUI>().MatchSpell(d.spellDeck[0]);
        this.transform.GetChild(1).GetComponent<CardUI>().MatchSpell(d.spellDeck[1]);
        this.transform.GetChild(2).GetComponent<CardUI>().MatchSpell(d.spellDeck[2]);
        this.transform.GetChild(3).GetComponent<CardUI>().MatchSpell(d.spellDeck[3]);
        this.transform.GetChild(4).GetComponent<CardUI>().MatchSpell(d.spellDeck[4]);
        this.transform.GetChild(5).GetComponent<CardUI>().MatchSpell(d.spellDeck[5]);
        this.transform.GetChild(6).GetComponent<CardUI>().MatchSpell(d.spellDeck[6]);
        this.transform.GetChild(7).GetComponent<CardUI>().MatchSpell(d.spellDeck[7]);
    }

    public void displayDeck(int i){
        SpellDeck d = decks[i];
        this.transform.GetChild(0).GetComponent<CardUI>().MatchSpell(d.spellDeck[0]);
        this.transform.GetChild(1).GetComponent<CardUI>().MatchSpell(d.spellDeck[1]);
        this.transform.GetChild(2).GetComponent<CardUI>().MatchSpell(d.spellDeck[2]);
        this.transform.GetChild(3).GetComponent<CardUI>().MatchSpell(d.spellDeck[3]);
        this.transform.GetChild(4).GetComponent<CardUI>().MatchSpell(d.spellDeck[4]);
        this.transform.GetChild(5).GetComponent<CardUI>().MatchSpell(d.spellDeck[5]);
        this.transform.GetChild(6).GetComponent<CardUI>().MatchSpell(d.spellDeck[6]);
        this.transform.GetChild(7).GetComponent<CardUI>().MatchSpell(d.spellDeck[7]);
    }
}
