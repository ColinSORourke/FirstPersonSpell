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
        for (int i = 0; i < 8; i++)
        {
            this.transform.GetChild(i + 1).GetComponent<CardUI>().MatchSpell(d.spellDeck[i]);
        }
    }

    public void displayDeck(int i){
        SpellDeck d = decks[i];
        for (int j = 0; j < 8; j++)
        {
            this.transform.GetChild(j + 1).GetComponent<CardUI>().MatchSpell(d.spellDeck[j]);
        }
    }
}
