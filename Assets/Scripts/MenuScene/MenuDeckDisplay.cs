using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDeckDisplay : MonoBehaviour
{
    public SpellDeck defaultDeck;
    public SpellDeck[] decks;
    public GameObject cardPrefab;

    public void Start(){
    
        
        int i = 0;
        while (i < 8){
            GameObject cardObj = Instantiate(cardPrefab, this.transform, false);
            var cardtrans = cardObj.GetComponent<RectTransform>();
            if (i > 3){
                cardtrans.anchoredPosition = new Vector2(-140 + (130 * (i - 4)),160);
            } else {
                cardtrans.anchoredPosition = new Vector2(-140 + (130 * i),350);
            }
            cardtrans.SetParent(this.transform); 
            i += 1;
        }

        this.displayDeck(defaultDeck);
        
    }

    public void displayDeck(SpellDeck d){
        for (int i = 0; i < d.spellDeck.Length; i++)
        {
            this.transform.GetChild(i + 1).GetComponent<CardUI>().MatchSpell(d.spellDeck[i]);
        }
        return;
        this.transform.GetChild(1).GetComponent<CardUI>().MatchSpell(d.spellDeck[0]);
        this.transform.GetChild(2).GetComponent<CardUI>().MatchSpell(d.spellDeck[1]);
        this.transform.GetChild(3).GetComponent<CardUI>().MatchSpell(d.spellDeck[2]);
        this.transform.GetChild(4).GetComponent<CardUI>().MatchSpell(d.spellDeck[3]);
        this.transform.GetChild(5).GetComponent<CardUI>().MatchSpell(d.spellDeck[4]);
        this.transform.GetChild(6).GetComponent<CardUI>().MatchSpell(d.spellDeck[5]);
        this.transform.GetChild(7).GetComponent<CardUI>().MatchSpell(d.spellDeck[6]);
        this.transform.GetChild(8).GetComponent<CardUI>().MatchSpell(d.spellDeck[7]);
    }

    public void displayDeck(int i){
        SpellDeck d = decks[i];
        this.transform.GetChild(1).GetComponent<CardUI>().MatchSpell(d.spellDeck[0]);
        this.transform.GetChild(2).GetComponent<CardUI>().MatchSpell(d.spellDeck[1]);
        this.transform.GetChild(3).GetComponent<CardUI>().MatchSpell(d.spellDeck[2]);
        this.transform.GetChild(4).GetComponent<CardUI>().MatchSpell(d.spellDeck[3]);
        this.transform.GetChild(5).GetComponent<CardUI>().MatchSpell(d.spellDeck[4]);
        this.transform.GetChild(6).GetComponent<CardUI>().MatchSpell(d.spellDeck[5]);
        this.transform.GetChild(7).GetComponent<CardUI>().MatchSpell(d.spellDeck[6]);
        this.transform.GetChild(8).GetComponent<CardUI>().MatchSpell(d.spellDeck[7]);
    }
}
